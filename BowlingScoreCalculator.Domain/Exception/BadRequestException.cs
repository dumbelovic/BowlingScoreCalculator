using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoreCalculator.Domain.Exception
{
    public class BadRequestException : System.Exception
    {
        public BadRequestException() 
            : base()
        {   
        }

        public BadRequestException(string message)
            : base(message)
        {

        }
    }
}
