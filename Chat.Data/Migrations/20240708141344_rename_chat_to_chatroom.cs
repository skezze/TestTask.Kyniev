using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Data.Migrations
{
    /// <inheritdoc />
    public partial class rename_chat_to_chatroom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatRoomId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chats",
                table: "Chats");

            migrationBuilder.RenameTable(
                name: "Chats",
                newName: "ChatRooms");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatRooms",
                table: "ChatRooms",
                column: "ChatRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatRooms_ChatRoomId",
                table: "Messages",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "ChatRoomId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatRooms_ChatRoomId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatRooms",
                table: "ChatRooms");

            migrationBuilder.RenameTable(
                name: "ChatRooms",
                newName: "Chats");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chats",
                table: "Chats",
                column: "ChatRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatRoomId",
                table: "Messages",
                column: "ChatRoomId",
                principalTable: "Chats",
                principalColumn: "ChatRoomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
