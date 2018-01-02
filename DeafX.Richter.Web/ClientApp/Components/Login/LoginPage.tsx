import * as React from 'react';
import TextInput from '../Shared/Input/TextInput'
//import { Device as DeviceModel, ToggleDevice } from "../../Models/Device";
//import { connect, Dispatch } from "react-redux";
//import { withRouter, RouteComponentProps } from "react-router-dom";
//import { ApplicationState } from "../../Store/ConfigureStore";
//import { DeviceState } from "../../Reducers/DeviceReducer";
//import { toggleDevice, setTimerDevice, setDeviceAutomated, ToggleDeviceAction, connectToDeviceApi, disconnectFromDeviceApi } from "../../Actions/DeviceActions"
//import TimerModal from "../Shared/TimerModalComponent";

interface LoginPageState {
    username: string,
    password: string
}

class LoginPage extends React.Component<any, LoginPageState> {

    constructor()
    {
        super();

        this.state = {
            username: "",
            password: ""
        };

        this.onTextInputChanged = this.onTextInputChanged.bind(this);
    }

    onTextInputChanged(target: string, event: any): void {
        this.setState(
            {
                ...this.state,
                [target]: event.target.value
            }
        )       
    }

    public render() {
        return <div className="loginPage">

            <div className="loginContainer">

                <TextInput value={this.state.username} icon="fa-user" onChange={this.onTextInputChanged} placeholder="Username" name="username" error="The username must contain atleast 6 characters" />

                <TextInput value={this.state.password} icon="fa-lock" password={true} onChange={this.onTextInputChanged} placeholder="Password" name="password"/>

                <div className="alert mb20">The is a form validation error message</div>

                <button className="btn">Log in</button>

            </div>

        </div>;
    }

}


export default LoginPage;