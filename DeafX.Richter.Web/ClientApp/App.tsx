import * as React from "react";
import * as ReactDOM from "react-dom"
import { AppContainer } from 'react-hot-loader';
import TestComponent from "./Components/TestComponent";

function renderApp() {
    ReactDOM.render(
        <AppContainer>
            <TestComponent/>
        </AppContainer>
        ,
        document.getElementById('react-app')
    );
}

renderApp();

// Allow Hot Module Replacement
if (module.hot) {
    module.hot.accept('./Components/TestComponent', () => {
        renderApp();
    });
}