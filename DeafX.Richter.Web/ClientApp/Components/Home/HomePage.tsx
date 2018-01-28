import * as React from 'react';
import Device from "./Device";
import { Device as DeviceModel, ToggleDevice } from "../../Models/Device";
import { connect, Dispatch } from "react-redux";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { ApplicationState } from "../../Store/ConfigureStore";
import { DeviceState } from "../../Reducers/DeviceReducer";
import { toggleDevice, setTimerDevice, abortTimerDevice, setDeviceAutomated, ToggleDeviceAction, connectToDeviceApi, disconnectFromDeviceApi } from "../../Actions/DeviceActions"
import TimerModal from "../Shared/TimerModalComponent";
import ConfirmationModal from "../Shared/ConfirmationModalComponent";

interface DeviceContainerStateProps
{
    devices: DeviceState
}

interface DeviceContainerActions {
    setTimerDevice,
    abortTimerDevice,
    toggleDevice,
    connectToDeviceApi,
    disconnectFromDeviceApi,
    setDeviceAutomated
}

interface DeviceContainerState {
    timerModalObject: ToggleDevice,
    abortTimerModalObject: ToggleDevice,
}

type DeviceContainerProps =
    DeviceContainerStateProps &
    DeviceContainerActions &
    RouteComponentProps<any>;

class HomePage extends React.Component<DeviceContainerProps, DeviceContainerState> {

    constructor()
    {
        super();
        this.onIconClick = this.onIconClick.bind(this);
        this.onConfigClick = this.onConfigClick.bind(this);
        this.onTimerClick = this.onTimerClick.bind(this);
        this.onTimerOk = this.onTimerOk.bind(this);
        this.onTimerCancel = this.onTimerCancel.bind(this);
        this.onAutomatedClick = this.onAutomatedClick.bind(this);
        this.onTimerAbortClick = this.onTimerAbortClick.bind(this);
        this.onTimerAbortOk = this.onTimerAbortOk.bind(this);
        this.onTimerAbortCancel = this.onTimerAbortCancel.bind(this);

        this.state = {
            abortTimerModalObject: null,
            timerModalObject: null,
        };
    }

    onIconClick(device: ToggleDevice): void {
        this.props.toggleDevice(device, !device.value);
    }

    onConfigClick(device: DeviceModel): void {
        this.props.history.push("/config/" + device.id);
    }

    onAutomatedClick(device: ToggleDevice): void {
        this.props.setDeviceAutomated(device, !device.automated);
    }

    onTimerClick(device: ToggleDevice): void {
        this.setState({ ...this.state, timerModalObject: device });
    }

    onTimerAbortClick(device: ToggleDevice): void {
        this.setState({ ...this.state, abortTimerModalObject: device });
    }

    onTimerAbortOk(): void {
        this.props.abortTimerDevice(this.state.abortTimerModalObject);
        this.setState({ ...this.state, abortTimerModalObject: null });
    }

    onTimerAbortCancel(): void {
        this.setState({ ...this.state, abortTimerModalObject: null });
    }

    onTimerOk(selectedTime: number, state: boolean): void {
        if (!selectedTime)
        {
            return;
        }

        this.props.setTimerDevice(this.state.timerModalObject, selectedTime, state);
        this.setState({ ...this.state, timerModalObject: null });
    }

    onTimerCancel(): void {
        this.setState({ ...this.state, timerModalObject: null });
    }

    public render() {
        return <div className="tileContainer">

            {this.props.devices.deviceList.map(function (device, index) {
                return <Device key={device.id} device={device as DeviceModel} onIconClick={this.onIconClick} onConfigClick={this.onConfigClick} onTimerClick={this.onTimerClick} onTimerAbortClick={this.onTimerAbortClick} onAutomatedClick={this.onAutomatedClick} />
            }, this)}

            {!!this.state.timerModalObject && <TimerModal onOkClick={this.onTimerOk} onCancelClick={this.onTimerCancel} />}
            {!!this.state.abortTimerModalObject && <ConfirmationModal onOkClick={this.onTimerAbortOk} onCancelClick={this.onTimerAbortCancel} title="Avbryt timer" message="Är du säker på att du vill avbryta timern?" />}

        </div>;
    }

    public componentWillMount() {
        this.props.connectToDeviceApi();
    }

    public componentWillUnmount() {
        this.props.disconnectFromDeviceApi();
    }

}

function mapStateToProps(state: ApplicationState, ownProps): DeviceContainerStateProps {
    return {
        devices: state.devices,
    }
}

function mapDispatchToProps(dispatch): DeviceContainerActions {
    return {
        setTimerDevice: (device: ToggleDevice, time: number, state: boolean) => dispatch(setTimerDevice(device, time, state)),
        abortTimerDevice: (device: ToggleDevice) => dispatch(abortTimerDevice(device)),
        toggleDevice: (device: ToggleDevice, toggled: boolean) => dispatch(toggleDevice(device, toggled)),
        connectToDeviceApi: () => dispatch(connectToDeviceApi()),
        disconnectFromDeviceApi: () => dispatch(disconnectFromDeviceApi()),
        setDeviceAutomated: (device: ToggleDevice, automated: boolean) => dispatch(setDeviceAutomated(device, automated))
    }
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(HomePage));