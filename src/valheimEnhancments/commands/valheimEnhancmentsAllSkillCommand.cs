using System;
using System.Collections.Generic;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsAllSkillCommand : valheimEnhancmentsCommand
    {
        public override string Name => "setskills";
        public override string Description => "Raises all skills to [value]";
        public override string Syntax => "none or [value]";

        public override void Execute(Terminal instance, List<string> arguments)
        {
            var value = 100f;

            if (arguments.Count > 0 && float.TryParse(arguments[0], out var argumentValue))
                value = argumentValue;

            var skills = Player.m_localPlayer.GetSkills();

            foreach (var currentSkill in skills.GetSkillList())
            {
                skills.CheatResetSkill(currentSkill.m_info.m_skill.ToString());
                skills.CheatRaiseSkill(currentSkill.m_info.m_skill.ToString(), value);
            }

            instance.AddString($"Raised all skills to level {value}");
        }
    }
}
