import * as React from 'react';
import { Device, DeviceProps } from "./Device";
import { Device as DeviceModel } from "../Models/Device";
import { connect, Dispatch } from "react-redux";
import { ApplicationState } from "../Store/ConfigureStore";
import { DeviceState } from "../Reducers/DeviceReducer";
import { toggleDevice, ToggleDeviceAction } from "../Actions/DeviceActions"

interface DeviceContainerStateProps
{
    devices: DeviceState
}

interface DeviceContainerActions {
    toggleDevice
}

type DeviceContainerProps =
    DeviceContainerStateProps &
    DeviceContainerActions;

class DeviceContainer extends React.Component<DeviceContainerProps, any> {

    onClick(device: DeviceModel): any {
        toggleDevice(device);
    }

    public render() {
        return <div className="tileContainer">

            {this.props.devices.deviceList.map(function (device, index) {
                return <Device key={device.id} device={device as DeviceModel} onClick={this.onClick} />
            }, this)}

        </div>;
    }

}

function mapStateToProps(state: ApplicationState, ownProps): DeviceContainerStateProps {
    return {
        devices: state.devices
    }
}

function mapDispatchToProps(dispatch): DeviceContainerActions {
    return {
        toggleDevice: (device: DeviceModel) => dispatch(toggleDevice(device))
    }
}

export default connect(mapStateToProps, )(DeviceContainer);