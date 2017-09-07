import * as React from "react";
import * as ReactDOM from "react-dom"
import { AppContainer } from 'react-hot-loader';
import DeviceContainer from "./Components/DeviceContainer";
import configureStore from "./Store/ConfigureStore";
import { Provider } from "react-redux";

const store = configureStore({
    devices: {
        deviceList: [
            {
                id: "1",
                title: "Vardagsrum",
                toggled: true
            },
            {
                id: "2",
                title: "Gästrum",
                toggled: false
            }
        ]
    }
})

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