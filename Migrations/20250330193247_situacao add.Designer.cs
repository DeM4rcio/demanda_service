﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace demanda_service.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250330193247_situacao add")]
    partial class situacaoadd
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Etapa", b =>
                {
                    b.Property<int>("EtapaProjetoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EtapaProjetoId"));

                    b.Property<string>("ANALISE")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("DT_INICIO_PREVISTO")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DT_INICIO_REAL")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DT_TERMINO_PREVISTO")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DT_TERMINO_REAL")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NM_ETAPA")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int>("NM_PROJETOprojetoId")
                        .HasColumnType("integer");

                    b.Property<decimal>("PERCENT_EXEC_ETAPA")
                        .HasColumnType("numeric");

                    b.Property<decimal>("PERCENT_TOTAL_ETAPA")
                        .HasColumnType("numeric");

                    b.Property<string>("RESPONSAVEL_ETAPA")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("SITUACAO")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("EtapaProjetoId");

                    b.HasIndex("NM_PROJETOprojetoId");

                    b.ToTable("Etapas");
                });

            modelBuilder.Entity("Models.AreaDemandante", b =>
                {
                    b.Property<int>("AreaDemandanteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AreaDemandanteID"));

                    b.Property<string>("NM_DEMANDANTE")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<string>("NM_SIGLA")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("AreaDemandanteID");

                    b.ToTable("AreaDemandantes");
                });

            modelBuilder.Entity("Models.Categoria", b =>
                {
                    b.Property<int>("CategoriaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CategoriaId"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("CategoriaId");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("Models.Demanda", b =>
                {
                    b.Property<int>("DemandaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DemandaId"));

                    b.Property<int>("CategoriaId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("DT_ABERTURA")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DT_CONCLUSAO")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DT_SOLICITACAO")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("NM_AREA_DEMANDANTEAreaDemandanteID")
                        .HasColumnType("integer");

                    b.Property<string>("NM_DEMANDA")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("NM_PO_DEMANDANTE")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("NM_PO_SUBTDCR")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("NR_PROCESSO_SEI")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.Property<string>("PATROCINADOR")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("PERIODICIDADE")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PERIODICO")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("STATUS")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("UNIDADE")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("DemandaId");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("NM_AREA_DEMANDANTEAreaDemandanteID");

                    b.ToTable("Demandas");
                });

            modelBuilder.Entity("Models.Projeto", b =>
                {
                    b.Property<int>("projetoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("projetoId"));

                    b.Property<string>("ANO")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("GERENTE_PROJETO")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<string>("NM_AREA_DEMANDANTE")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<string>("NM_PROJETO")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("NR_PROCESSO_SEI")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<bool>("PDTIC2427")
                        .HasColumnType("boolean");

                    b.Property<bool>("PROFISCOII")
                        .HasColumnType("boolean");

                    b.Property<bool>("PTD2427")
                        .HasColumnType("boolean");

                    b.Property<string>("SITUACAO")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<string>("TEMPLATE")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("UNIDADE")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.HasKey("projetoId");

                    b.ToTable("Projetos");
                });

            modelBuilder.Entity("Models.Template", b =>
                {
                    b.Property<int>("TemplateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TemplateId"));

                    b.Property<string>("NM_ETAPA")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("NM_TEMPLATE")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<decimal>("PERCENT_TOTAL")
                        .HasMaxLength(100)
                        .HasColumnType("numeric");

                    b.HasKey("TemplateId");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("Etapa", b =>
                {
                    b.HasOne("Models.Projeto", "NM_PROJETO")
                        .WithMany()
                        .HasForeignKey("NM_PROJETOprojetoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NM_PROJETO");
                });

            modelBuilder.Entity("Models.Demanda", b =>
                {
                    b.HasOne("Models.Categoria", "CATEGORIA")
                        .WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.AreaDemandante", "NM_AREA_DEMANDANTE")
                        .WithMany()
                        .HasForeignKey("NM_AREA_DEMANDANTEAreaDemandanteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CATEGORIA");

                    b.Navigation("NM_AREA_DEMANDANTE");
                });
#pragma warning restore 612, 618
        }
    }
}
