using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaEfler.Models
{
    public class RepositorioContrato
    {
        protected readonly string connectionString;
        public RepositorioContrato()
        {
            connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria_efler;SslMode=none";
        }
        public int AltaContrato(Contrato c)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO contrato (fecha_desde,fecha_hasta,monto_alquiler,pago_al_dia,id_inmueble,id_inquilino) 
                VALUES (@fecha_desde,@fecha_hasta,@monto_alquiler,@pago_al_dia,@id_inmueble,@id_inquilino); 
                SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@fecha_desde", c.FechaDesde);
                    command.Parameters.AddWithValue("@fecha_hasta", c.FechaHasta);
                    command.Parameters.AddWithValue("@monto_alquiler", c.MontoAlquiler);
                    command.Parameters.AddWithValue("@pago_al_dia", c.PagoAlDia);
                    command.Parameters.AddWithValue("@id_inmueble", c.IdInmueble);
                    command.Parameters.AddWithValue("@id_inquilino", c.IdInquilino);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    c.Id = res;
                    connection.Close();
                }
            }
            return res;
        }
        public int BajaContrato(int id)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = $"DELETE FROM contrato WHERE id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        public int ModificacionContrato(Contrato c)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE contrato SET fecha_desde= @fecha_desde, 
                fecha_hasta= @fecha_hasta,monto_alquiler= @monto_alquiler,
                pago_al_dia=@pago_al_dia,id_inmueble= @id_inmueble,id_inquilino=@id_inquilino 
                WHERE id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", c.Id);
                    command.Parameters.AddWithValue("@fecha_desde", c.FechaDesde);
                    command.Parameters.AddWithValue("@fecha_hasta", c.FechaHasta);
                    command.Parameters.AddWithValue("@monto_alquiler", c.MontoAlquiler);
                    command.Parameters.AddWithValue("@pago_al_dia", c.PagoAlDia);
                    command.Parameters.AddWithValue("@id_inmueble", c.IdInmueble);
                    command.Parameters.AddWithValue("@id_inquilino", c.IdInquilino);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        public List<Contrato> ObtenerContratos()
        {
            List<Contrato> res = new List<Contrato>();
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.id, fecha_desde, fecha_hasta,monto_alquiler,pago_al_dia,id_inmueble,
                id_inquilino,i.direccion,inq.nombre,inq.apellido 
                FROM contrato c 
                JOIN inmueble i ON (i.id =c.id_inmueble) 
                JOIN inquilino inq ON (inq.id =c.id_inquilino);";
                using (var comm = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaDesde = reader.GetDateTime(1),
                            FechaHasta = reader.GetDateTime(2),
                            MontoAlquiler = reader.GetDecimal(3),
                            PagoAlDia = reader.GetBoolean(4),
                            IdInmueble = reader.GetInt32(5),
                            IdInquilino = reader.GetInt32(6),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(7),
                            },
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
                            }

                        });
                    }
                    conn.Close();
                }
            }
            return res;
        }
        public Contrato ObtenerPorId(int id)
        {
            Contrato c = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.id, fecha_desde, fecha_hasta,monto_alquiler,pago_al_dia,id_inmueble,
                id_inquilino,i.direccion,inq.nombre,inq.apellido 
                FROM contrato c 
                JOIN inmueble i ON (i.id =c.id_inmueble) 
                JOIN inquilino inq ON (inq.id =c.id_inquilino)
                WHERE c.id= @id;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        c = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaDesde = reader.GetDateTime(1),
                            FechaHasta = reader.GetDateTime(2),
                            MontoAlquiler = reader.GetDecimal(3),
                            PagoAlDia = reader.GetBoolean(4),
                            IdInmueble = reader.GetInt32(5),
                            IdInquilino = reader.GetInt32(6),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(7),
                            },
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return c;
        }

    }

}
