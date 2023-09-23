using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetWeb.Migrations
{
    public partial class data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "Accepted", "Comment", "Content", "CustomerId", "Delivered", "Destination", "DriverId", "EndDate", "Source", "StartDate", "TruckId" },
                values: new object[,]
                {
                    { 50, false, null, "20 tonnes de poisson", "2f150cc6-6312-454b-b772-310c126409a5", false, "", null, new DateTime(2024, 1, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), "", new DateTime(2023, 1, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 51, false, null, "un chat", "2f150cc6-6312-454b-b772-310c126409a5", false, "", null, new DateTime(2024, 2, 6, 13, 10, 0, 0, DateTimeKind.Unspecified), "", new DateTime(2023, 2, 6, 13, 10, 0, 0, DateTimeKind.Unspecified), null },
                    { 52, false, null, "10 tonnes de bois", "2f150cc6-6312-454b-b772-310c126409a5", false, "", null, new DateTime(2024, 3, 7, 14, 20, 0, 0, DateTimeKind.Unspecified), "", new DateTime(2023, 3, 7, 14, 20, 0, 0, DateTimeKind.Unspecified), null },
                    { 53, false, null, "des tables et chaises pour helmo", "39168414-951b-4c21-8127-9c396f6a63fc", false, "", null, new DateTime(2024, 4, 8, 15, 30, 0, 0, DateTimeKind.Unspecified), "", new DateTime(2023, 4, 8, 15, 30, 0, 0, DateTimeKind.Unspecified), null },
                    { 54, false, null, "une bombe nucléaire", "3bb23f40-700e-4b43-a3a0-f178e5181897", false, "", null, new DateTime(2024, 5, 9, 16, 40, 0, 0, DateTimeKind.Unspecified), "", new DateTime(2023, 5, 9, 16, 40, 0, 0, DateTimeKind.Unspecified), null },
                    { 55, false, null, "20 tonnes de poisson", "3bb23f40-700e-4b43-a3a0-f178e5181897", false, "", null, new DateTime(2024, 6, 10, 17, 50, 0, 0, DateTimeKind.Unspecified), "", new DateTime(2023, 6, 10, 17, 50, 0, 0, DateTimeKind.Unspecified), null },
                    { 56, false, null, "un chat", "3bb23f40-700e-4b43-a3a0-f178e5181897", false, "", null, new DateTime(2024, 7, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), "", new DateTime(2023, 7, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 57, false, null, "10 tonnes de bois", "3bb23f40-700e-4b43-a3a0-f178e5181897", false, "", null, new DateTime(2024, 8, 12, 19, 10, 0, 0, DateTimeKind.Unspecified), "", new DateTime(2023, 8, 12, 19, 10, 0, 0, DateTimeKind.Unspecified), null },
                    { 58, false, null, "des tables et chaises pour helmo", "cd8eb13c-cb9e-436c-b05a-9a3130a0bc04", false, "", null, new DateTime(2024, 9, 13, 20, 20, 0, 0, DateTimeKind.Unspecified), "", new DateTime(2023, 9, 13, 20, 20, 0, 0, DateTimeKind.Unspecified), null },
                    { 59, false, null, "une bombe nucléaire", "cd8eb13c-cb9e-436c-b05a-9a3130a0bc04", false, "", null, new DateTime(2024, 10, 14, 21, 30, 0, 0, DateTimeKind.Unspecified), "", new DateTime(2023, 10, 14, 21, 30, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "Trucks",
                columns: new[] { "Id", "Brand", "MaxWeight", "Model", "Plate", "Types" },
                values: new object[,]
                {
                    { 1, "Iveco", 4500, "Eurocargo", "1-IJZ-676", "C" },
                    { 2, "Iveco", 1000, "Eurocargo", "1-IEW-184", "C" },
                    { 3, "Iveco", 4400, "Eurocargo", "1-IHX-764", "C" },
                    { 4, "Iveco", 6600, "Eurocargo", "1-QQR-218", "CE" },
                    { 5, "Iveco", 4700, "Eurocargo", "1-HOC-254", "CE" },
                    { 6, "Iveco", 5000, "Eurocargo", "1-VUD-421", "CE" },
                    { 7, "Iveco", 2900, "Eurocargo", "1-EMB-710", "C" },
                    { 8, "Iveco", 2400, "Eurocargo", "1-DNQ-121", "CE" },
                    { 9, "Iveco", 1800, "Eurocargo", "1-VBH-107", "C" },
                    { 10, "Iveco", 5300, "Eurocargo", "1-ZTF-135", "C" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Trucks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Trucks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Trucks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Trucks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Trucks",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Trucks",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Trucks",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Trucks",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Trucks",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Trucks",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
