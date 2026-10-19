using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP224_Kadrgaliev
{
    internal abstract class Person
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string Contact { get; private set; }

        protected Person(int id, string name, int age, string contact)
        {
            if (id <= 0) throw new ArgumentException("Номер должен быть положительным.");
            if (age < 1 || age > 100) throw new ArgumentException("Возраст должен быть от 1 до 100 лет.");
            ID = id;
            Name = Validation.RequiredText(name, "ФИО");
            Age = age;
            Contact = Validation.RequiredText(contact, "Контактная информация");
        }

        public abstract string GetInfo();
    }

    internal sealed class Student : Person
    {
        public string Group { get; private set; }
        public Student(int id, string name, int age, string contact, string group)
            : base(id, name, age, contact)
        {
            Group = Validation.RequiredText(group, "Группа");
        }
        public override string GetInfo()
        {
            return $"Студент #{ID}: {Name}; возраст: {Age}; контакт: {Contact}; группа: {Group}";
        }
    }

    internal sealed class Teacher : Person
    {
        public string Specialization { get; private set; }
        public Teacher(int id, string name, int age, string contact, string specialization)
            : base(id, name, age, contact)
        {
            Specialization = Validation.RequiredText(specialization, "Специализация");
        }
        public override string GetInfo()
        {
            return $"Преподаватель #{ID}: {Name}; возраст: {Age}; контакт: {Contact}; специализация: {Specialization}";
        }
    }

    internal static class Validation
    {
        public static string RequiredText(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException(fieldName + ": значение не должно быть пустым.");
            string result = value.Trim();
            if (result.Length > 250) throw new ArgumentException(fieldName + ": максимум 250 символов.");
            return result;
        }
    }

    internal sealed class Course
    {
        private readonly List<Student> students = new List<Student>();
        public int ID { get; private set; }
        public string Title { get; private set; }
        public Teacher Teacher { get; private set; }
        public IReadOnlyList<Student> Students { get { return students.ToArray(); } }
        public Course(int id, string title, Teacher teacher)
        {
            if (id <= 0) throw new ArgumentException("Номер курса должен быть положительным.");
            ID = id;
            Title = Validation.RequiredText(title, "Название курса");
            AssignTeacher(teacher);
        }
        public void AssignTeacher(Teacher teacher)
        {
            if (teacher == null) throw new ArgumentException("У курса должен быть преподаватель.");
            Teacher = teacher;
        }
        public bool EnrollStudent(Student student)
        {
            if (student == null) throw new ArgumentException("Студент не выбран.");
            if (students.Any(existing => existing.ID == student.ID)) return false;
            students.Add(student);
            return true;
        }
        public string GetInfo()
        {
            return $"Курс #{ID}: {Title}; преподаватель: {Teacher.Name} (#{Teacher.ID}); студентов: {students.Count}";
        }
    }

    internal sealed class University
    {
        private readonly List<Student> students = new List<Student>();
        private readonly List<Teacher> teachers = new List<Teacher>();
        private readonly List<Course> courses = new List<Course>();
        public IReadOnlyList<Student> Students{get { return students.ToArray();}}
        public IReadOnlyList<Teacher> Teachers{get { return teachers.ToArray(); }}
        public IReadOnlyList<Course> Courses{get { return courses.ToArray(); }}
        private int nextPersonID = 1;
        private int nextCourseID = 1;
        public Student AddStudent(string name, int age, string contact, string group)
        {
            Student student = new Student(nextPersonID, name, age, contact, group);
            students.Add(student);
            nextPersonID++;
            return student;
        }
        public Teacher AddTeacher(string name, int age, string contact, string specialization)
        {
            Teacher teacher = new Teacher(nextPersonID, name, age, contact, specialization);
            teachers.Add(teacher);
            nextPersonID++;
            return teacher;
        }
        public Course AddCourse(string title, int teacherId)
        {
            string normalizedTitle = Validation.RequiredText(title, "Название курса");
            if (courses.Any(course => string.Equals(course.Title, normalizedTitle,StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Курс с таким названием уже существует.");
            }

            Course newCourse = new Course(nextCourseID, normalizedTitle, GetTeacher(teacherId));
            courses.Add(newCourse);
            nextCourseID++;
            return newCourse;
        }

        public Student GetStudent(int id)
        {
            Student student = students.FirstOrDefault(item => item.ID == id);
            if (student == null) throw new InvalidOperationException("Студент с таким номером не найден.");
            return student;
        }

        public Teacher GetTeacher(int id)
        {
            Teacher teacher = teachers.FirstOrDefault(item => item.ID == id);

            if (teacher == null)
                throw new InvalidOperationException("Преподаватель с таким номером не найден.");

            return teacher;
        }
        public Course GetCourse(int id)
        {
            Course course = courses.FirstOrDefault(item => item.ID == id);
            if (course == null) throw new InvalidOperationException("Курс с таким номером не найден.");
            return course;
        }
        public bool EnrollStudent(int studentId, int courseId)
        {
            return GetCourse(courseId).EnrollStudent(GetStudent(studentId));
        }
        public void AssignTeacher(int courseId, int teacherId)
        {
            GetCourse(courseId).AssignTeacher(GetTeacher(teacherId));
        }
        public List<Course> GetStudentCourses(int studentId)
        {
            GetStudent(studentId);
            return courses.Where(course => course.Students.Any(student => student.ID == studentId)).OrderBy(course => course.Title).ToList();
        }
        public List<Course> GetTeacherCourses(int teacherId)
        {
            GetTeacher(teacherId);
            return courses.Where(course => course.Teacher.ID == teacherId).OrderBy(course => course.Title).ToList();
        }
    }

    internal class Program
    {
        private static readonly University university = new University();
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            while (true)
            {
                Menu();
                Console.Write("Выберите пункт: ");
                string choice = Console.ReadLine();
                if (choice == null)
                {
                    Console.WriteLine("Ввод завершён. Работа программы окончена.");
                    return;
                }
                try
                {
                    switch (choice.Trim())
                    {
                        case "0":
                            Console.WriteLine("Работа завершена.");
                            return;
                        case "1":
                            {
                                Console.Write("ФИО: ");
                                string name = Validation.RequiredText(Console.ReadLine(), "ФИО");
                                Console.Write("Возраст (1–100): ");
                                int age;
                                if (!int.TryParse(Console.ReadLine(), out age) || age < 1 || age > 100)
                                {
                                    Console.WriteLine("Введите целый возраст от 1 до 100 лет.");
                                    break;
                                }
                                Console.Write("Контакт (телефон, email или другой способ связи): ");
                                string contact = Validation.RequiredText(Console.ReadLine(), "Контактная информация");
                                Console.Write("Группа: ");
                                string group = Validation.RequiredText(Console.ReadLine(), "Группа");
                                Student student = university.AddStudent(name, age, contact, group);
                                Console.WriteLine("Студент добавлен. Его номер: " + student.ID);
                                break;
                            }
                        case "2":
                            {
                                if (university.Students.Count == 0)
                                {
                                    Console.WriteLine("Сначала добавьте студента через пункт 1.");
                                    break;
                                }
                                Console.Write("Номер студента: ");
                                int studentId;
                                if (!int.TryParse(Console.ReadLine(), out studentId) || studentId <= 0)
                                {
                                    Console.WriteLine("Номер должен быть положительным целым числом.");
                                    break;
                                }
                                Person person = university.GetStudent(studentId);
                                Console.WriteLine(person.GetInfo());
                                break;
                            }
                        case "3":
                            {
                                if (university.Students.Count == 0)
                                {
                                    Console.WriteLine("Список пуст.");
                                    break;
                                }
                                foreach (Person person in university.Students.OrderBy(item => item.Name).ThenBy(item => item.ID))
                                {
                                    Console.WriteLine(person.GetInfo());
                                }
                                break;
                            }
                        case "4":
                            {
                                Console.Write("ФИО: ");
                                string name = Validation.RequiredText(Console.ReadLine(), "ФИО");
                                Console.Write("Возраст (1–100): ");
                                int age;
                                if (!int.TryParse(Console.ReadLine(), out age) || age < 1 || age > 100)
                                {
                                    Console.WriteLine("Введите целый возраст от 1 до 100 лет.");
                                    break;
                                }
                                Console.Write("Контакт (телефон, email или другой способ связи): ");
                                string contact = Validation.RequiredText(Console.ReadLine(), "Контактная информация");
                                Console.Write("Специализация: ");
                                string specialization = Validation.RequiredText(Console.ReadLine(), "Специализация");
                                Teacher teacher = university.AddTeacher(name, age, contact, specialization);
                                Console.WriteLine("Преподаватель добавлен. Его номер: " + teacher.ID);
                                break;
                            }
                        case "5":
                            {
                                if (university.Teachers.Count == 0)
                                {
                                    Console.WriteLine("Сначала добавьте преподавателя через пункт 4.");
                                    break;
                                }
                                Console.Write("Номер преподавателя: ");
                                int teacherId;
                                if (!int.TryParse(Console.ReadLine(), out teacherId) || teacherId <= 0)
                                {
                                    Console.WriteLine("Номер должен быть положительным целым числом.");
                                    break;
                                }
                                Person person = university.GetTeacher(teacherId);
                                Console.WriteLine(person.GetInfo());
                                Console.WriteLine("Курсы преподавателя:");
                                List<Course> courses = university.GetTeacherCourses(teacherId);
                                if (courses.Count == 0) Console.WriteLine("Список курсов пуст.");
                                foreach (Course course in courses) Console.WriteLine(course.GetInfo());
                                break;
                            }
                        case "6":
                            {
                                if (university.Teachers.Count == 0)
                                {
                                    Console.WriteLine("Список пуст.");
                                    break;
                                }
                                foreach (Person person in university.Teachers.OrderBy(item => item.Name).ThenBy(item => item.ID))
                                {
                                    Console.WriteLine(person.GetInfo());
                                }
                                break;
                            }
                        case "7":
                            {
                                if (university.Teachers.Count == 0)
                                {
                                    Console.WriteLine("Сначала добавьте преподавателя через пункт 4.");
                                    break;
                                }
                                foreach (Person person in university.Teachers.OrderBy(item => item.Name).ThenBy(item => item.ID))
                                {
                                    Console.WriteLine(person.GetInfo());
                                }
                                Console.Write("Номер преподавателя: ");
                                int teacherId;
                                if (!int.TryParse(Console.ReadLine(), out teacherId) || teacherId <= 0)
                                {
                                    Console.WriteLine("Номер должен быть положительным целым числом.");
                                    break;
                                }
                                university.GetTeacher(teacherId);
                                Console.Write("Название курса: ");
                                string title = Validation.RequiredText(Console.ReadLine(), "Название курса");
                                Course course = university.AddCourse(title, teacherId);
                                Console.WriteLine("Курс создан. Его номер: " + course.ID);
                                break;
                            }
                        case "8":
                            {
                                if (university.Courses.Count == 0)
                                {
                                    Console.WriteLine("Сначала создайте курс через пункт 7.");
                                    break;
                                }
                                Console.Write("Номер курса: ");
                                int courseId;
                                if (!int.TryParse(Console.ReadLine(), out courseId) || courseId <= 0)
                                {
                                    Console.WriteLine("Номер должен быть положительным целым числом.");
                                    break;
                                }
                                Course course = university.GetCourse(courseId);
                                Console.WriteLine(course.GetInfo());
                                Console.WriteLine("Преподаватель курса:");
                                Person teacher = course.Teacher;
                                Console.WriteLine(teacher.GetInfo());
                                Console.WriteLine("Записанные студенты:");
                                if (course.Students.Count == 0) Console.WriteLine("Список пуст.");
                                foreach (Person person in course.Students.OrderBy(item => item.Name).ThenBy(item => item.ID))
                                {
                                    Console.WriteLine(person.GetInfo());
                                }
                                break;
                            }
                        case "9":
                            {
                                if (university.Courses.Count == 0)
                                {
                                    Console.WriteLine("Список курсов пуст.");
                                    break;
                                }
                                foreach (Course course in university.Courses.OrderBy(item => item.Title).ThenBy(item => item.ID))
                                {
                                    Console.WriteLine(course.GetInfo());
                                }
                                break;
                            }
                        case "10":
                            {
                                if (university.Students.Count == 0)
                                {
                                    Console.WriteLine("Сначала добавьте студента через пункт 1.");
                                    break;
                                }
                                if (university.Courses.Count == 0)
                                {
                                    Console.WriteLine("Сначала создайте курс через пункт 7.");
                                    break;
                                }
                                foreach (Person person in university.Students.OrderBy(item => item.Name).ThenBy(item => item.ID))
                                {
                                    Console.WriteLine(person.GetInfo());
                                }
                                Console.Write("Номер студента: ");
                                int studentId;
                                if (!int.TryParse(Console.ReadLine(), out studentId) || studentId <= 0)
                                {
                                    Console.WriteLine("Номер должен быть положительным целым числом.");
                                    break;
                                }
                                university.GetStudent(studentId);
                                foreach (Course course in university.Courses.OrderBy(item => item.Title).ThenBy(item => item.ID))
                                {
                                    Console.WriteLine(course.GetInfo());
                                }
                                Console.Write("Номер курса: ");
                                int courseId;
                                if (!int.TryParse(Console.ReadLine(), out courseId) || courseId <= 0)
                                {
                                    Console.WriteLine("Номер должен быть положительным целым числом.");
                                    break;
                                }
                                bool enrolled = university.EnrollStudent(studentId, courseId);
                                Console.WriteLine(enrolled? "Студент записан на курс.": "Студент уже записан на этот курс.");
                                break;
                            }
                        case "11":
                            {
                                if (university.Students.Count == 0)
                                {
                                    Console.WriteLine("Сначала добавьте студента через пункт 1.");
                                    break;
                                }
                                Console.Write("Номер студента: ");
                                int studentId;
                                if (!int.TryParse(Console.ReadLine(), out studentId) || studentId <= 0)
                                {
                                    Console.WriteLine("Номер должен быть положительным целым числом.");
                                    break;
                                }
                                Person person = university.GetStudent(studentId);
                                Console.WriteLine(person.GetInfo());
                                Console.WriteLine("Курсы студента:");
                                List<Course> courses = university.GetStudentCourses(studentId);
                                if (courses.Count == 0) Console.WriteLine("Список курсов пуст.");
                                foreach (Course course in courses) Console.WriteLine(course.GetInfo());
                                break;
                            }
                        case "12":
                            {
                                if (university.Courses.Count == 0)
                                {
                                    Console.WriteLine("Сначала создайте курс через пункт 7.");
                                    break;
                                }
                                if (university.Teachers.Count == 0)
                                {
                                    Console.WriteLine("Сначала добавьте преподавателя через пункт 4.");
                                    break;
                                }
                                foreach (Course course in university.Courses.OrderBy(item => item.Title).ThenBy(item => item.ID))
                                {
                                    Console.WriteLine(course.GetInfo());
                                }
                                Console.Write("Номер курса: ");
                                int courseId;
                                if (!int.TryParse(Console.ReadLine(), out courseId) || courseId <= 0)
                                {
                                    Console.WriteLine("Номер должен быть положительным целым числом.");
                                    break;
                                }
                                university.GetCourse(courseId);
                                foreach (Person person in university.Teachers.OrderBy(item => item.Name).ThenBy(item => item.ID))
                                {
                                    Console.WriteLine(person.GetInfo());
                                }
                                Console.Write("Номер преподавателя: ");
                                int teacherId;
                                if (!int.TryParse(Console.ReadLine(), out teacherId) || teacherId <= 0)
                                {
                                    Console.WriteLine("Номер должен быть положительным целым числом.");
                                    break;
                                }
                                university.AssignTeacher(courseId, teacherId);
                                Console.WriteLine("Преподаватель назначен.");
                                break;
                            }
                        default:
                            Console.WriteLine("Выберите пункт от 0 до 12.");
                            break;
                    }
                }
                catch (ArgumentException exception)
                {
                    Console.WriteLine("Ошибка: " + exception.Message);
                }
                catch (InvalidOperationException exception)
                {
                    Console.WriteLine("Ошибка: " + exception.Message);
                }
            }
        }

        private static void Menu()
        {
            Console.WriteLine("\n========== УНИВЕРСИТЕТ ==========");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Информация о студенте");
            Console.WriteLine("3. Все студенты");
            Console.WriteLine("4. Добавить преподавателя");
            Console.WriteLine("5. Информация о преподавателе");
            Console.WriteLine("6. Все преподаватели");
            Console.WriteLine("7. Создать курс");
            Console.WriteLine("8. Информация о курсе");
            Console.WriteLine("9. Все курсы");
            Console.WriteLine("10. Записать студента на курс");
            Console.WriteLine("11. Курсы студента");
            Console.WriteLine("12. Назначить или заменить преподавателя курса");
            Console.WriteLine("0. Выход");
        }
    }
}