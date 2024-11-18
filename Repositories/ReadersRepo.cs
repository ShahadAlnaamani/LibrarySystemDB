using LibrarySystemDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemDB.Repositories
{
    public class ReadersRepo
    {
        private readonly ApplicationDBContext _context;
        public ReadersRepo(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<Reader> GetAll()
        {
            return _context.Readers.ToList();
        }

        public Reader GetReaderByID(int ID)
        {
            return _context.Readers.Find(ID);
        }

        public Reader GetReaderByName(string name)
        {
            return _context.Readers.FirstOrDefault(u => u.RFName == name || u.RLName == name);
        }

        public int GetReaderIDByName(string name)
        {
           var reader = _context.Readers.Find(name);
            return reader.RID;
        }

        public Reader GetReaderByUserName(string user)
        {
            return _context.Readers.FirstOrDefault(u => u.RUserName == user);
        }

        public void Update(string name)
        {
            var reader = GetReaderByName(name);
            if (reader != null)
            {
                _context.Readers.Update(reader);
                _context.SaveChanges();
            }
        }

        public void Add(string FName, string LName, string email, GenderType gender, string phone, string UName, string pass)
        {
            var reader = new Reader {RFName = FName, RLName = LName, REmail = email, RGender = gender, RPhoneNo = phone, RUserName = UName, Password = pass};

            _context.Readers.Add(reader);
            _context.SaveChanges();
        }

        public void Delete(int ID)
        {
            var reader = GetReaderByID(ID);
            if (reader != null)
            {
                _context.Readers.Remove(reader);
                _context.SaveChanges();
            }
        }

        public int CountByGender(GenderType gender)
        {
           return _context.Readers.Where(g=> g.RGender == gender).Count();
        }

        public int UserAuthentication(string UserName, string Password)
        {
            var reader = GetReaderByUserName(UserName);
            if (reader != null)
            {
                if (reader.Password == Password)
                {
                    
                    return 1; //successful login
                }

                else 
                {
                    return 2; //account found but wrong pass
                }
            }

            else 
            {
                //call sign up function 
                return 3; //wrong username 
            }
        }
    }
}
