using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using ParallelStringProcessing.Classes;
namespace StringProcessingAPI.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        //[Route("api/values/{s}/{operations}")]
        public string Get([FromUri] string s, [FromUri] int[] operations)
        {
            StringBuilder tmp = new StringBuilder("");
            var result = new StringBuilder(s);
            foreach (var op in operations)
            {
                tmp.Append(op.ToString());
                ParseCommand(ref result, (StringOperations) op);
            }
            return result.ToString();
        }        

        private void ParseCommand(ref StringBuilder line, StringOperations command)
        {
            switch (command)
            {
                case StringOperations.Uppercase:
                    UpperCase(ref line);
                    break;

                case StringOperations.Sort:
                    Sort(ref line);
                    break;

                case StringOperations.LowerCase:
                    LowerCase(ref line);
                    break;

                case StringOperations.Invert:
                    Invert(ref line);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void Invert(ref StringBuilder s)
        {
            for (int i = 0, j = s.Length - 1; i < j; i++, j--)
            {
                (s[i], s[j]) = (s[j], s[i]);
            }
        }

        private void LowerCase(ref StringBuilder s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = char.ToLower(s[i]);
            }
        }

        private void Sort(ref StringBuilder s)
        {
            var sortedLetters = s.ToString().ToCharArray();
            Array.Sort(sortedLetters);
            s.Clear();
            s.Append(new string(sortedLetters));
        }

        private void UpperCase(ref StringBuilder s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = char.ToUpper(s[i]);
            }
        }

        public string ProcessString(string s, int[] operations)
        {
            return "worked";
        }
        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
