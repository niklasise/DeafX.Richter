export interface AppConfiguration {
    apiUrl: string;
}

class ConfigurationUtility {

    public static getConfiguration() : AppConfiguration{
        return (window as any).ReactAppConfiguration as AppConfiguration;
    }

}

export default ConfigurationUtility;