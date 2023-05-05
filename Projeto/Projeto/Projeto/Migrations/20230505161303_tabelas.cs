using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto.Migrations
{
    public partial class tabelas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Establishments",
                table: "Establishments");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Establishments");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Establishments");

            migrationBuilder.RenameTable(
                name: "Establishments",
                newName: "Establishment");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "EstablishmentType",
                table: "Establishment",
                newName: "UserFK");

            migrationBuilder.AddColumn<int>(
                name: "TypeEstablishment",
                table: "Establishment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Establishment",
                table: "Establishment",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Denounced = table.Column<bool>(type: "bit", nullable: false),
                    IsAnswer = table.Column<bool>(type: "bit", nullable: false),
                    EstablishmentFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Establishment_EstablishmentFK",
                        column: x => x.EstablishmentFK,
                        principalTable: "Establishment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstablishmentsRate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Stars = table.Column<int>(type: "int", nullable: false),
                    UserFK = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EstablishmentFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstablishmentsRate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstablishmentsRate_Establishment_EstablishmentFK",
                        column: x => x.EstablishmentFK,
                        principalTable: "Establishment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstablishmentsRate_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstablishmentFK = table.Column<int>(type: "int", nullable: false),
                    EstablishmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photo_Establishment_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentRate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    CommentFK = table.Column<int>(type: "int", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    UserFK = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentRate", x => x.id);
                    table.ForeignKey(
                        name: "FK_CommentRate_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentRate_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Establishment_UserFK",
                table: "Establishment",
                column: "UserFK");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_EstablishmentFK",
                table: "Comment",
                column: "EstablishmentFK");

            migrationBuilder.CreateIndex(
                name: "IX_CommentRate_CommentId",
                table: "CommentRate",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentRate_UserId",
                table: "CommentRate",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EstablishmentsRate_EstablishmentFK",
                table: "EstablishmentsRate",
                column: "EstablishmentFK");

            migrationBuilder.CreateIndex(
                name: "IX_EstablishmentsRate_UserId",
                table: "EstablishmentsRate",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_EstablishmentId",
                table: "Photo",
                column: "EstablishmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Establishment_Users_UserFK",
                table: "Establishment",
                column: "UserFK",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishment_Users_UserFK",
                table: "Establishment");

            migrationBuilder.DropTable(
                name: "CommentRate");

            migrationBuilder.DropTable(
                name: "EstablishmentsRate");

            migrationBuilder.DropTable(
                name: "Photo");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Establishment",
                table: "Establishment");

            migrationBuilder.DropIndex(
                name: "IX_Establishment_UserFK",
                table: "Establishment");

            migrationBuilder.DropColumn(
                name: "TypeEstablishment",
                table: "Establishment");

            migrationBuilder.RenameTable(
                name: "Establishment",
                newName: "Establishments");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "UserFK",
                table: "Establishments",
                newName: "EstablishmentType");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Establishments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Establishments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Establishments",
                table: "Establishments",
                column: "Id");
        }
    }
}
