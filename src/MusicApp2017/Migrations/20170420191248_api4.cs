using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicApp2017.Migrations
{
    public partial class api4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Albums",
                newName: "RatingNumber");

            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Albums",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Albums");

            migrationBuilder.RenameColumn(
                name: "RatingNumber",
                table: "Albums",
                newName: "Rating");
        }
    }
}
