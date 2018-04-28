import * as React from "react";
import * as ReactDOM from "react-dom"
import { AppContainer } from 'react-hot-loader';
import configureStore from "./store/ConfigureStore";
import { Provider } from "react-redux";
import { BrowserRouter } from 'react-router-dom'
import { routes } from "./Routes";
import mockApi from "./Api/mockDeviceApi";

const store = configureStore({
    devices: {
        deviceList: []
    },
    logs: {
        clientLogs: [],
        serverLogs: [],
        currentErrorMessage: null
    }
})

function renderApp() {
    ReactDOM.render(
            <Provider store={store}>
                <BrowserRouter children={routes} />
            </Provider>
        ,
        document.getElementById('react-app')
    );
}

renderApp();
