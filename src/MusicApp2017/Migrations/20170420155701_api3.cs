using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicApp2017.Migrations
{
    public partial class api3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ratingNumber",
                table: "Rating",
                newName: "RatingNumber");

            migrationBuilder.AlterColumn<double>(
                name: "RatingNumber",
                table: "Rating",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RatingNumber",
                table: "Rating",
                newName: "ratingNumber");

            migrationBuilder.AlterColumn<int>(
                name: "ratingNumber",
                table: "Rating",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
