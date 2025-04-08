using System.Text.Json;
using InterviewTestQA.InterviewTestAutomation;
using Newtonsoft.Json;


namespace InterviewTestQA
{
    public class JSONTest
    {
        [Fact]
        public void Test1()
        {         
            string workingDirectory = Environment.CurrentDirectory;

            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            String relativePath = projectDirectory + "\\\\InterviewTestAutomation\\\\Data\\\\";
            Console.WriteLine("\n\nFile Path : " + relativePath);

            string filePath = Path.Combine(projectDirectory, relativePath + "Cost Analysis.json");

            String fileContent = "";
            if (File.Exists(filePath))
            {
                 fileContent = File.ReadAllText(filePath);               
            }

            // -----  Deserialise the json file into your list ----------------------------------------------//

            List<CostAnalysis> ca= JsonConvert.DeserializeObject<List<CostAnalysis>>(fileContent);


                Console.WriteLine("1st item in list \n");
                Console.WriteLine("YearId : " + ca[0].YearId);
                Console.WriteLine("GeoRegionId : " + ca[0].GeoRegionId);
                Console.WriteLine("CountryId : " + ca[0].CountryId);
                Console.WriteLine("RegionId : " + ca[0].RegionId);
                Console.WriteLine("SchemeId : " + ca[0].SchemeId);
                Console.WriteLine("SchmTypeId : " + ca[0].SchmTypeId);
                Console.WriteLine("Cost : " + ca[0].Cost);

            // -----  Write to Assert how many items are in your list ----------------------------------------//

            Assert.Equal(53, ca.Count);

            // -----  Get the top item ordered by Cost descending, and write to Assert the CountryId.---------//

            var costDesc = ca.OrderByDescending(item => item.Cost).Take(1);
            Assert.Equal(0, costDesc.First().CountryId);

            // -----  Sum Cost for 2016 and write to Assert the total.  --------------------------------------//

            var sumCost = ca.Where(item => item.YearId.Equals("2016"));
            double count = 0;
            foreach (var item in sumCost)
            {
                count += item.Cost;
            }
            Assert.Equal(77911.3744561, count);
        }
    }
}