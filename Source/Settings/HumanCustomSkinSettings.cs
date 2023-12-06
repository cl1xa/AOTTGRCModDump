namespace Settings;

internal class HumanCustomSkinSettings : BaseCustomSkinSettings<HumanCustomSkinSet>
{
	public BoolSetting GasEnabled = new BoolSetting(defaultValue: true);

	public BoolSetting HookEnabled = new BoolSetting(defaultValue: true);
}
