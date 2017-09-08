import * as React from "react";
import * as ReactDOM from "react-dom"
import { AppContainer } from 'react-hot-loader';
import DeviceContainer from "./Components/DeviceContainer";
import configureStore from "./Store/ConfigureStore";
import { Provider } from "react-redux";
import { loadDevices } from "./Actions/DeviceActions";

const store = configureStore({
    devices: {
        deviceList: []
    }
})
store.dispatch(loadDevices());

function renderApp() {
    ReactDOM.render(
        <AppContainer>
            <Provider store={store}>
                <DeviceContainer />
            </Provider>
        </AppContainer>
        ,
        document.getElementById('react-app')
    );
}

renderApp();

// Allow Hot Module Replacement
if (module.hot) {
    module.hot.accept('./Components/DeviceContainer', () => {
        renderApp();
    });
}