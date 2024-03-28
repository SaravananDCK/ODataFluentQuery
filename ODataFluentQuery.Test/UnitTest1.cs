namespace ODataFluentQuery.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //var str= new ODataQuery<Student>().Filter(u => u.DOB.LessThanOrEquals($"{new DateTime(2010, 1, 1):yyyy-MM-ddTHH:mm:ssZ}")).ToString();
            //var str2= new ODataQuery<Student>().Filter(u => u.Name.StartsWith($"Harry")).ToString();

            var odataFilter = new ODataFilter<Student>()
                .FilterBy(p=>p.Teacher.FirstName=="Adam")
                .And()
                .BeginGroup()
                    .FilterBy(p => p.FirstName == "Harry")
                    .Or()
                    .FilterBy(p => p.Section == "Grade V")
                .EndGroup()
                .And()
                .FilterBy(p => p.Age > 10)
                .And()
                .BeginGroup()
                .And()
                .Contains(p=>p.LastName,"Smith")
                .Or()
                .StartsWith(p => p.LastName, "Smi")
                .EndGroup().Build();
            if (odataFilter == "")
            {

            }
        }
    }

    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Section { get; set; }
        public DateTime DOB { get; set; }
        public int Age { get; set; }
        public Teacher Teacher { get; set; }
    }
    public class Teacher
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Section { get; set; }
        public DateTime DOB { get; set; }
    }
}