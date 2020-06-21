using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using NSE.Clientes.API.Models;
using NSE.Core.DomainObjects;
using System.Xml.Linq;

namespace NSE.Clientes.API.Data.Mappings
{
    public class ClienteMapping : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            //Abordagem que dita que o CPF pertence ao cliente
            //Cpf não é um dado, é uma estrutura, uma class
            //Estamos mapeando o número e não a class
            builder.OwnsOne(c => c.Cpf, tf =>
            {
                tf.Property(c => c.Numero)
                    .IsRequired()
                    .HasMaxLength(Cpf.CpfMaxLength)
                    .HasColumnName("Cpf")
                    .HasColumnType($"varchar({Cpf.CpfMaxLength})");
            });

            builder.OwnsOne(c => c.Email, tf =>
            {
                tf.Property(c => c.Endereco)
                    .IsRequired()
                    .HasColumnName("Email")
                    .HasColumnType($"varchar({Email.EnderecoMaxLength})");
            });

            // 1 : 1 =: Aluno : Endereco
            builder.HasOne(c => c.Endereco)
                .WithOne(c => c.Cliente);

            builder.ToTable("Clientes");

        }
    }
}
