using ServerForReact.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForReact.Exceptions
{
    public class CourseException : Exception
    {
        public CourseError CourseError { get; private set; }
        public CourseException(CourseError courseError)
        {
            CourseError = courseError;
        }
    }
}
