import * as React from 'react';
import { Link } from 'react-router-dom'
import { connect, Dispatch } from "react-redux";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { ApplicationState } from "../Store/ConfigureStore";
import { LogState } from "../Reducers/LogReducer";
import { ClientLog } from "../Models/Log/ClientLog";
import { addClientError } from "../Actions/LogActions";


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
               message: message,
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
        return <div className="h100">
            {this.props.children}
            <div className={!!this.props.logs.currentErrorMessage ? "errorToaster show" : "errorToaster hide"}>{this.props.logs.currentErrorMessage}</div>
        </div>;
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

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(App));