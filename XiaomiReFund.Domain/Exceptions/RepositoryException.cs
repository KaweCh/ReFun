using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaomiReFund.Domain.Exceptions
{
    /// <summary>
    /// ข้อผิดพลาดเกี่ยวกับการเข้าถึงคลังข้อมูล
    /// </summary>
    public class RepositoryException : DomainException
    {
        /// <summary>
        /// ชื่อของคลังข้อมูล
        /// </summary>
        public string RepositoryName { get; }

        /// <summary>
        /// ชื่อของเมธอด
        /// </summary>
        public string MethodName { get; }

        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยข้อความ
        /// </summary>
        /// <param name="message">ข้อความแสดงข้อผิดพลาด</param>
        public RepositoryException(string message) : base(message)
        {
        }

        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยชื่อคลังข้อมูล ชื่อเมธอด และข้อความ
        /// </summary>
        /// <param name="repositoryName">ชื่อคลังข้อมูล</param>
        /// <param name="methodName">ชื่อเมธอด</param>
        /// <param name="message">ข้อความแสดงข้อผิดพลาด</param>
        public RepositoryException(string repositoryName, string methodName, string message)
            : base($"เกิดข้อผิดพลาดในคลังข้อมูล '{repositoryName}' ที่เมธอด '{methodName}': {message}")
        {
            RepositoryName = repositoryName;
            MethodName = methodName;
        }

        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยข้อความและข้อผิดพลาดต้นเหตุ
        /// </summary>
        /// <param name="message">ข้อความแสดงข้อผิดพลาด</param>
        /// <param name="innerException">ข้อผิดพลาดต้นเหตุ</param>
        public RepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// สร้างข้อผิดพลาดใหม่ด้วยชื่อคลังข้อมูล ชื่อเมธอด ข้อความ และข้อผิดพลาดต้นเหตุ
        /// </summary>
        /// <param name="repositoryName">ชื่อคลังข้อมูล</param>
        /// <param name="methodName">ชื่อเมธอด</param>
        /// <param name="message">ข้อความแสดงข้อผิดพลาด</param>
        /// <param name="innerException">ข้อผิดพลาดต้นเหตุ</param>
        public RepositoryException(string repositoryName, string methodName, string message, Exception innerException)
            : base($"เกิดข้อผิดพลาดในคลังข้อมูล '{repositoryName}' ที่เมธอด '{methodName}': {message}", innerException)
        {
            RepositoryName = repositoryName;
            MethodName = methodName;
        }
    }
}
