import * as React from 'react';
import { Device, DeviceProps } from "./Device";
import { Device as DeviceModel, ToggleDevice } from "../../Models/Device";
import { connect, Dispatch } from "react-redux";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { ApplicationState } from "../../Store/ConfigureStore";
import { DeviceState } from "../../Reducers/DeviceReducer";
import { toggleDevice, ToggleDeviceAction, loadDevicesAndListenForUpdates, stopListeningForDeviceUpdates } from "../../Actions/DeviceActions"

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

type DeviceContainerProps =
    DeviceContainerStateProps &
    DeviceContainerActions &
    RouteComponentProps<any>;

class HomePage extends React.Component<DeviceContainerProps, any> {

    constructor()
    {
        super();
        this.onIconClick = this.onIconClick.bind(this);
        this.onConfigClick = this.onConfigClick.bind(this);
    }

    onIconClick(device: DeviceModel): void {
        this.props.toggleDevice(device);
    }

    onConfigClick(device: DeviceModel): void {
        this.props.history.push("/config/" + device.id);
    }
    
    public render() {
        return <div className="tileContainer">

            {this.props.devices.deviceList.map(function (device, index) {
                return <Device key={device.id} device={device as DeviceModel} onIconClick={this.onIconClick} onConfigClick={this.onConfigClick} />
            }, this)}

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