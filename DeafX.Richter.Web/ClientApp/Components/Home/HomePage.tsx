import * as React from 'react';
import { Device, DeviceProps } from "./Device";
import { Device as DeviceModel, ToggleDevice } from "../../Models/Device";
import { connect, Dispatch } from "react-redux";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { ApplicationState } from "../../Store/ConfigureStore";
import { DeviceState } from "../../Reducers/DeviceReducer";
import { toggleDevice, ToggleDeviceAction, loadDevicesAndListenForUpdates, stopListeningForDeviceUpdates } from "../../Actions/DeviceActions"
import TimerModal from "../Shared/TimerModalComponent";

interface DeviceContainerStateProps
{
    devices: DeviceState
}

interface DeviceContainerBlah {
    history
}

interface DeviceContainerActions {
    toggleDevice,
    loadDevices,
    stopDeviceUpdates
}

interface DeviceContainerState {
    timerModalObject: DeviceModel
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
        this.onTimerReset = this.onTimerReset.bind(this);
        this.onTimerCancel = this.onTimerCancel.bind(this);

        this.state = {
            timerModalObject: null
        };
    }

    onIconClick(device: DeviceModel): void {
        this.props.toggleDevice(device);
    }

    onConfigClick(device: DeviceModel): void {
        this.props.history.push("/config/" + device.id);
    }

    onTimerClick(device: DeviceModel): void {
        this.setState({ ...this.state, timerModalObject: device })
    }

    onTimerOk(selectedTime: number): void {
        this.setState({ ...this.state, timerModalObject: null })
    }

    onTimerCancel(): void {
        this.setState({ ...this.state, timerModalObject: null })
    }

    onTimerReset(): void {
        this.setState({ ...this.state, timerModalObject: null })
    }

    public render() {
        return <div className="tileContainer">

            {this.props.devices.deviceList.map(function (device, index) {
                return <Device key={device.id} device={device as DeviceModel} onIconClick={this.onIconClick} onConfigClick={this.onConfigClick} onTimerClick={this.onTimerClick} />
            }, this)}

            {!!this.state.timerModalObject && <TimerModal initialTime={0} onOkClick={this.onTimerOk} onCancelClick={this.onTimerCancel} onResetClick={this.onTimerReset} />}

        </div>;
    }

    public componentWillMount() {
        this.props.loadDevices();
    }

    public componentWillUnmount() {
        this.props.stopDeviceUpdates();
    }

}

function mapStateToProps(state: ApplicationState, ownProps): DeviceContainerStateProps {
    return {
        devices: state.devices,
    }
}

function mapDispatchToProps(dispatch): DeviceContainerActions {
    return {
        toggleDevice: (device: ToggleDevice) => dispatch(toggleDevice(device)),
        loadDevices: () => dispatch(loadDevicesAndListenForUpdates()),
        stopDeviceUpdates: () => dispatch(stopListeningForDeviceUpdates())
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(HomePage));