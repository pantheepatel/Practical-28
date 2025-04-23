using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExceptionLogger;
using ExceptionLogger.Models;
using ExceptionLogger.Repository.StudentRepo;
using LogException.UnitOfWork;

namespace ExceptionLogger.Controllers
{
    public class StudentController(IUnitOfWork unitOfWork, ILogger<StudentController> logger) : Controller
    {
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Index()
        {
            logger.LogInformation("Fetching all students");
            var students = await unitOfWork.Students.GetAllAsync();
            var studentVMs = unitOfWork.mapper.Map<List<StudentViewModel>>(students);
            return View(studentVMs);
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Details(int id)
        {
            logger.LogInformation($"Fetching details for student with ID: {id}");
            var student = await unitOfWork.Students.GetByIdAsync(id);
            if (student == null) return NotFound();
            var viewModel = unitOfWork.mapper.Map<StudentViewModel>(student);
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,EnrollmentNo,Email,Course,AdmissionDate")] Student studentData)
        {
            logger.LogInformation("Creating a new student");
            var student = unitOfWork.mapper.Map<Student>(studentData);
            await unitOfWork.Students.AddAsync(student);
            await unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await unitOfWork.Students.GetByIdAsync(id);
            if (student == null) return NotFound();
            var viewModel = unitOfWork.mapper.Map<StudentViewModel>(student);
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,EnrollmentNo,Email,Course,AdmissionDate")] Student viewModel)
        {
            if (id != viewModel.Id)
            {
                logger.LogWarning($"ID mismatch: {id} != {viewModel.Id}");
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                logger.LogWarning("Model state is invalid");
                return View(viewModel);
            }

            var student = unitOfWork.mapper.Map<Student>(viewModel);
            logger.LogInformation($"Updating student with ID: {id}");
            await unitOfWork.Students.UpdateAsync(student);
            await unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                logger.LogWarning("ID is null");
                return NotFound();
            }

            var student = await unitOfWork.Students.GetByIdAsync(id);
            if (student == null)
            {
                logger.LogWarning($"Student with ID: {id} not found");
                return NotFound();
            }

            return View(student);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            logger.LogInformation($"Deleting student with ID: {id}");
            await unitOfWork.Students.DeleteAsync(id);
            await unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }
    }
}
