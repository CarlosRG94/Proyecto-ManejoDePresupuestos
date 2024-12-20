﻿using Dapper;
using ManejoDePresupuestos.Models;
using Microsoft.Data.SqlClient;

namespace ManejoDePresupuestos.Servicios
{
    public class RepositorioCategorias:IRepositorioCategorias
    {
        private readonly string _connectionString;
        public RepositorioCategorias(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Crear(Categoria categoria)
        {
            using var connection = new SqlConnection(_connectionString);
            var id = await connection.QuerySingleAsync<int>(@"
             INSERT INTO Categorias (Nombre,TipoOperacionId,UsuarioId)
             VALUES (@Nombre,@TipoOperacionId,@UsuarioId);
             SELECT SCOPE_IDENTITY();", categoria);
            categoria.Id = id;
        }
        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId,
            PaginacionViewModel paginacion)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Categoria>(@$"
                    SELECT * FROM Categorias
                    WHERE UsuarioId = @usuarioId
                    ORDER BY Nombre
                    OFFSET {paginacion.RecordsASaltar} ROWS FETCH NEXT {paginacion.RecordsPorPagina}
                    ROWS ONLY", new { usuarioId });
            
        }

        public async Task<int> Contar (int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Categorias WHERE UsuarioId = @usuarioId", new { usuarioId }
                );
        }
        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId,TipoOperacion tipoOperacionId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Categoria>(@"
                 SELECT * 
                 FROM Categorias
                WHERE UsuarioId = @usuarioId AND TipoOperacionId = @tipoOperacionId;",
                 new { usuarioId,tipoOperacionId });
        }
        public async Task<Categoria> ObtenerPorId(int id,int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Categoria>(@"
             SELECT * FROM Categorias WHERE Id = @id AND UsuarioId = @usuarioId;",
             new { id, usuarioId });
        }
        public async Task Editar(Categoria categoria)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(@"UPDATE Categorias
               SET Nombre = @Nombre, TipoOperacionId = @TipoOperacionId
               Where Id = @Id",categoria);
        }
        public async Task Borrar (int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(@"DELETE Categorias WHERE Id = @id",
                new { id });
        }
    }
}
