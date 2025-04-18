using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CzechUp.EF.Migrations
{
    /// <inheritdoc />
    public partial class FillRuleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "Rules",
               columns: new[] { "Name" },
               values: new object[,]
               {
                    { "Podstatná jména - Rod substantiv" },
                    { "Podstatná jména - Význam pádů" },
                    { "Podstatná jména - Nominativ singuláru" },
                    { "Podstatná jména - Nominativ plurálu" },
                    { "Podstatná jména - Genitiv singuláru" },
                    { "Podstatná jména - Genitiv plurálu" },
                    { "Podstatná jména - Dativ singuláru a plurálu" },
                    { "Podstatná jména - Akuzativ singuláru" },
                    { "Podstatná jména - Akuzativ plurálu" },
                    { "Podstatná jména - Vokativ singuláru a plurálu" },
                    { "Podstatná jména - Lokál singuláru a plurálu" },
                    { "Podstatná jména - Instrumentál singuláru a plurálu" },
                    { "Podstatná jména - Deklinace v singuláru" },
                    { "Podstatná jména - Deklinace v plurálu" },
                    { "Přídavná jména a číslovky - Adjektiva tvrdá a měkká" },
                    { "Přídavná jména a číslovky - Adjektiva přivlastňovací" },
                    { "Přídavná jména a číslovky - Číslovky základní" },
                    { "Přídavná jména a číslovky - Číslovky řadové" },
                    { "Zájmena - Zájmena osobní" },
                    { "Zájmena - Zájmena přivlastňovací" },
                    { "Zájmena - Zájmena tázací" },
                    { "Zájmena - Zájmena neučitá" },
                    { "Tvorba slov a vět - Slovosled 1" },
                    { "Tvorba slov a vět - Slovosled 2" },
                    { "Tvorba slov a vět - Přechylování" },
                    { "Ostatní - Výslovnost" },
                    { "Ostatní - Hodiny" },
                    { "Ostatní - Datum" },
                    { "Ostatní - Dny v týdnu" },
                    { "Ostatní - Adverbia směru" },
                    { "Slovesa - Formální a neformální oslovení" },
                    { "Slovesa - Sloveso být" },
                    { "Slovesa - Přítomný čas" },
                    { "Slovesa - Minulý čas" },
                    { "Slovesa - Futurum nedokonavých sloves" },
                    { "Slovesa - Futurum dokonavých sloves" },
                    { "Slovesa - Imperativ" },
                    { "Slovesa - Kondicionál" },
                    { "Slovesa - Zvratná slovesa" },
                    { "Slovesa - Modální slovesa" },
                    { "Slovesa - Mám rád/rád dělám" },

               });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
