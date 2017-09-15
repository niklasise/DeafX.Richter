import * as React from "react";
import * as ReactDOM from "react-dom"
import { AppContainer } from 'react-hot-loader';
import configureStore from "./Store/ConfigureStore";
import { Provider } from "react-redux";
import { BrowserRouter } from 'react-router-dom'
import { routes } from "./Routes";
import mockApi from "./Api/mockDeviceApi";

const store = configureStore({
    devices: {
        deviceList: []
    }
})
mockApi.startRandomizingValueDevices();

function renderApp() {
    ReactDOM.render(
        <AppContainer>
            <Provider store={store}>
                <BrowserRouter children={routes} />
            </Provider>
        </AppContainer>
        ,
        document.getElementById('react-app')
    );
}

renderApp();

// Allow Hot Module Replacement
if (module.hot) {
    module.hot.accept('./Routes', () => {
        renderApp();
    });
}