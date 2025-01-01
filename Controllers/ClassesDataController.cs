using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.MVC.Data;

namespace SchoolManagement.MVC.Controllers
{
    public class ClassesDataController : Controller
    {
        private readonly SchoolManagementDbContext _context;

        public ClassesDataController(SchoolManagementDbContext context)
        {
            _context = context;
        }

        // GET: ClassesData
        public async Task<IActionResult> Index()
        {
            // select * from classes_data as c left join Courses as co on c.CourseId = co.Id left join Lecturers  as l on c.LecturerId = l.Id
            var schoolManagementDbContext = _context.ClassesData.Include(c => c.Course).Include(c => c.Lecturer);
            return View(await schoolManagementDbContext.ToListAsync());
        }

        // GET: ClassesData/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classesDatum = await _context.ClassesData
                .Include(c => c.Course)
                .Include(c => c.Lecturer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classesDatum == null)
            {
                return NotFound();
            }

            return View(classesDatum);
        }

        // GET: ClassesData/Create
        public IActionResult Create()
        {
            CreateSelectLists();
            return View();
        }

        // POST: ClassesData/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LecturerId,CourseId,Time")] ClassesDatum classesDatum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classesDatum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            CreateSelectLists();
            return View(classesDatum);
        }

        // GET: ClassesData/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classesDatum = await _context.ClassesData.FindAsync(id);
            if (classesDatum == null)
            {
                return NotFound();
            }
           CreateSelectLists();
            return View(classesDatum);
        }

        // POST: ClassesData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LecturerId,CourseId,Time")] ClassesDatum classesDatum)
        {
            if (id != classesDatum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classesDatum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassesDatumExists(classesDatum.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            CreateSelectLists();
            return View(classesDatum);
        }

        // GET: ClassesData/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classesDatum = await _context.ClassesData
                .Include(c => c.Course)
                .Include(c => c.Lecturer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classesDatum == null)
            {
                return NotFound();
            }

            return View(classesDatum);
        }

        // POST: ClassesData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classesDatum = await _context.ClassesData.FindAsync(id);
            if (classesDatum != null)
            {
                _context.ClassesData.Remove(classesDatum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult>ManageEnrollments(int id){
             var classesDatum = await _context.ClassesData
                .Include(c => c.Course)
                .Include(c => c.Lecturer)
                .Include(c=> c.Enrollments)
                    .ThenInclude(c=>c.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            var students = await _context.Students.ToListAsync();
            var model = new ClassEnrollmentVM();
            model.ClassesDatum = classesDatum;

            foreach(var stu in students){
                model.Students.Add(new StudentEnrollmentVM{
                    Id = stu.Id,
                    FirstName = stu.FisrtName,
                    LastName = stu.LastName,
                    IsEnrolled = (classesDatum.Enrollments?.Any(q => q.StudentId == stu.Id))
                        .GetValueOrDefault()
                });
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnrollStudent(int classId, int studentId, bool shouldEnroll){
            var enrollment = new Enrollment();
            if(shouldEnroll == true)
            {
                enrollment.ClassId =classId;
                enrollment.StudentId =studentId;    
                await _context.AddAsync(enrollment);
            }
            else{
                enrollment = await _context.Enrollments.FirstOrDefaultAsync(q => q.ClassId == classId && q.StudentId == studentId);
                if(enrollment != null){
                    _context.Remove(enrollment);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageEnrollments),new {id=classId});
        }

        private bool ClassesDatumExists(int id)
        {
            return _context.ClassesData.Any(e => e.Id == id);
        }
        private void CreateSelectLists(){
            var courses = _context.Courses.Select(course =>new{
                Course_Code_Name =$"{course.Code}-{course.Name}",
                course.Id
            });
            ViewData["CourseId"] = new SelectList(courses, "Id", "Course_Code_Name");// fetch Courseid and code-name to select one of them
            var lecturers = _context.Lecturers.Select(q => new{
                Fullname = $"{q.FisrtName} {q.LastName}",
                q.Id
            });
            ViewData["LecturerId"] = new SelectList(lecturers, "Id", "Fullname");// provide list of lecturerid to select
        }
    }
}
