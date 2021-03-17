using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer
{
    public class ComponentDefinitions
    {
        public static readonly ComponentDefinition Client = new ComponentDefinition(InstallationComponent.Client, "Client", "PMDShift-C.exe", "PMD Shift!", "Pokémon Mystery Dungeon: Shift!");
        public static readonly ComponentDefinition Editor = new ComponentDefinition(InstallationComponent.Editor, "Editor", "Eagle.exe", "PMD Shift! - Editors", "Pokémon Mystery Dungeon: Shift! Editors");
        public static readonly ComponentDefinition LegacyClient = new ComponentDefinition(InstallationComponent.LegacyClient, "LegacyClient", "PMDCP.exe", "PMD Shift! - Legacy", "Pokémon Mystery Dungeon: Shift! - Legacy Client");
    }
}
