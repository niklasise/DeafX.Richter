﻿import * as React from 'react';
import { Link } from 'react-router-dom'
import { connect, Dispatch } from "react-redux";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { hot } from 'react-hot-loader';
import { ApplicationState } from "../Store/ConfigureStore";
import { LogState } from "../Reducers/LogReducer";
import { ClientLog } from "../Models/Log/ClientLog";
import { addClientError } from "../Actions/LogActions";
import styled from "styled-components";

const StyledDiv = styled.div`
    height: 100%;
`


interface AppState{
    logs: LogState
}

interface AppActions {
    addClientError
}

type AppProps =
    AppState &
    AppActions &
    RouteComponentProps<any>;

class App extends React.Component<AppProps, any> {

    public componentWillMount() {
        this.startErrorLog();
    }

    private startErrorLog() {
        window.onerror = (message, file, line, column, errorObject) => {
           var stack = errorObject ? errorObject.stack : null;

           var data: ClientLog = {
               timestamp: Date.now(),
               message: message.toString(),
               file: file,
               line: line,
               column: column,
               errorStack: stack,
            };

           this.props.addClientError(data);

           return false;
        }
    }


    public render() {
        return <StyledDiv>
            {this.props.children}
            <div className={!!this.props.logs.currentErrorMessage ? "errorToaster show" : "errorToaster hide"}>{this.props.logs.currentErrorMessage}</div>
        </StyledDiv>;
    }

}

function mapStateToProps(state: ApplicationState, ownProps): AppState {
    return {
        logs: state.logs,
    }
}

function mapDispatchToProps(dispatch): AppActions {
    return {
        addClientError: (data: ClientLog) => dispatch(addClientError(data))
    }
}

export default hot(module)(withRouter(connect(mapStateToProps, mapDispatchToProps)(App)));