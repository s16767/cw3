using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw3.DTOs;
using cw3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Services
{
    public class SqlServerDbService : IStudentsDbService
    {

        private string conString = "Data Source=db-mssql;Initial Catalog=s16767;Integrated Security=True";
        public Enrollment EnrollStudent(EnrollStudentRequest request)
        {
            Enrollment enrollment = new Enrollment();

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand())
            {

                command.Connection = connection;
                connection.Open();

                var transaction = connection.BeginTransaction();

                try
                {
                    command.Transaction = transaction;

                    command.CommandText = "SELECT IdStudy " +
                                          "FROM Studies " +
                                          "WHERE name=@name";
                    command.Parameters.AddWithValue("name", request.Studies);

                    var dr = command.ExecuteReader();

                    if (!dr.Read())
                    {
                        dr.Close();
                        transaction.Rollback();
                        return null;
                    }

                    int idStudy = (int)dr["IdStudy"];

                    command.CommandText = "SELECT TOP(1) IdEnrollment " +
                                          "FROM Enrollment " +
                                          "WHERE IdStudy=@IdStudy AND Semester=1 " +
                                          "ORDER BY StartDate DESC";
                    command.Parameters.AddWithValue("IdStudy", idStudy);

                    dr.Close();

                    dr = command.ExecuteReader();

                    if (!dr.Read())
                    {
                        command.CommandText = "INSERT INTO Enrollment " +
                                              "VALUES( SELECT MAX(IdEnrollment) FROM ENROLLMENT, 1, @IdStudy, GETDATE())";
                        command.Parameters.AddWithValue("IdStudy", idStudy);
                        command.ExecuteNonQuery();
                    }

                    command.CommandText = "SELECT IndexNumber " +
                                          "FROM Student " +
                                          "WHERE IndexNumber=@IndexNumber";
                    command.Parameters.AddWithValue("IndexNumber", request.IndexNumber);

                    dr.Close();

                    dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        dr.Close();
                        transaction.Rollback();
                        return null;
                    }

                    command.CommandText = "SELECT MAX(IdEnrollment) " +
                                          "FROM Enrollment";

                    dr.Close();

                    dr = command.ExecuteReader();
                    dr.Read();

                    int idEnrollment = (int)dr[0];

                    dr.Close();

                    command.CommandText = "INSERT INTO Student " +
                                          "VALUES(@IndexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment)";
                    command.Parameters.AddWithValue("FirstName", request.FirstName);
                    command.Parameters.AddWithValue("LastName", request.LastName);
                    command.Parameters.AddWithValue("BirthDate", request.BirthDate);
                    command.Parameters.AddWithValue("IdEnrollment", idEnrollment);

                    command.ExecuteNonQuery();

                    command.CommandText = "SELECT StartDate " +
                                          "FROM Enrollment " +
                                          "WHERE IdEnrollment=@IdEnrollment";

                    dr = command.ExecuteReader();
                    dr.Read();

                    string startDate = dr["StartDate"].ToString();

                    dr.Close();

                    transaction.Commit();

                    enrollment = new Enrollment("1", request.Studies, startDate);
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    transaction.Rollback();
                }

            }
            return enrollment;
        }

        public string PromoteStudent(PromoteStudentRequest request)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand())
            {

                command.Connection = connection;
                connection.Open();

                var transaction = connection.BeginTransaction();

                try
                {
                    command.Transaction = transaction;

                    command.CommandText = "SELECT IdEnrollment " +
                                          "FROM Enrollment " +
                                          "WHERE Semester=@Semester AND IdStudy=( SELECT IdStudy " +
                                                                                 "FROM Studies " +
                                                                                 "WHERE Name=@Name)";
                    command.Parameters.AddWithValue("Semester", request.Semester);
                    command.Parameters.AddWithValue("Name", request.Studies);

                    var dr = command.ExecuteReader();

                    if (!dr.Read())
                    {
                        dr.Close();
                        transaction.Rollback();
                        return NotFound("Nie znaleziono takiego wpisu");
                    }
                    dr.Close();
                    transaction.Commit();

                    using (SqlCommand cmd = new SqlCommand("Promotion", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Studies", SqlDbType.VarChar).Value = request.Studies;
                        cmd.Parameters.Add("@Semester", SqlDbType.Int).Value = request.Semester;

                        cmd.ExecuteNonQuery();
                    }




                }
                catch (SqlException e)
                {
                    transaction.Rollback();
                    return "Cos poszlo nie tak";
                }
            }

            return "Zaktualizowano wpisy";
        }
    }
}
