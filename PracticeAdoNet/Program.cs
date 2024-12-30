using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeAdoNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ClassCal ado = new ClassCal();
            Console.WriteLine("enter number");

            PostModel post = new PostModel();

            post.Id = 1;
            post.KeyVal = "test-my-block";
            post.Title = "Testblock2";
            post.Author = "mboin";
            post.Section = "This is the section place";
            post.YearOfPost = DateTime.Now.ToString(); post.Year = 1995;post.Month = 2;

            int number = Convert.ToInt32(Console.ReadLine());
            //dynamic ouput;
            switch (number)
            {
                case 1:
                    ado.ExecuteStoredProc();
                    break;
                case 2:
                    Console.WriteLine(ado.ReadRecords(1).ToString());
                    break;
                case 3:
                    ado.DeleteRecords(1);
                    break;
                case 4:
                    ado.UpdateRecords(3);
                    break;
                case 5:
                    ado.CreateRecords(post);
                    break;
                case 6:
                    ado.ReturnStoredProc();
                    break;
                case 7:
                    Console.WriteLine(ado.ReturnScalrValFunction(3));
                    break;
                case 8:
                    Console.WriteLine(ado.ReturnTableFromFunc(4).ToString());
                    break;
            }
            Console.ReadKey();

        }
    }
}
