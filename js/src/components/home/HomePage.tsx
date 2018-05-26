import * as React from 'react';
import ToggleDevice from "./devices/ToggleDevice";
import ValueDevice from "./devices/ValueDevice";
import WeatherAir from "./devices/WeatherAirDevice";
import WeatherWindDevice from "./devices/WeatherWindDevice";
import SunDevice from "./devices/SunDevice";
import { Device as DeviceModel, ToggleDevice as ToggleDeviceModel, WeatherAirDevice as WeatherAirDeviceModel, WeatherWindDevice as WeatherWindDeviceModel, SunDevice as SunDeviceViewModel } from "models/device";
import { connect, Dispatch } from "react-redux";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { ApplicationState } from "store/ConfigureStore";
import { DeviceState } from "reducers/DeviceReducer";
import { toggleDevice, setTimerDevice, abortTimerDevice, setDeviceAutomated, ToggleDeviceAction, connectToDeviceApi, disconnectFromDeviceApi } from "../../Actions/DeviceActions"
import TimerModal from "components/shared/TimerModalComponent";
import ConfirmationModal from "components/shared/ConfirmationModalComponent";
import styled from "styled-components";
import styles from "constants/styles";

const ContainerDiv = styled.div`
    max-width: 900px;
    padding: 20px 20px 0 20px;
    overflow: hidden;
    margin: 0 auto;

    @media screen and (max-width: ${styles.breakpoints.large}) {
        padding: 15px 15px 0 15px;
    }

    @media screen and (max-width: ${styles.breakpoints.medium}) {
        padding: 10px 10px 0 10px;
    }
`

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
    timerModalObject: ToggleDeviceModel,
    abortTimerModalObject: ToggleDeviceModel,
}

type DeviceContainerProps =
    DeviceContainerStateProps &
    DeviceContainerActions &
    RouteComponentProps<any>;

class HomePage extends React.Component<DeviceContainerProps, DeviceContainerState> {

    constructor(props: DeviceContainerProps)
    {
        super(props);
        this.onIconClick = this.onIconClick.bind(this);
        this.onConfigClick = this.onConfigClick.bind(this);
        this.onTimerClick = this.onTimerClick.bind(this);
        this.onTimerOk = this.onTimerOk.bind(this);
        this.onTimerCancel = this.onTimerCancel.bind(this);
        this.onAutomatedClick = this.onAutomatedClick.bind(this);
        this.onTimerAbortClick = this.onTimerAbortClick.bind(this);
        this.onTimerAbortOk = this.onTimerAbortOk.bind(this);
        this.onTimerAbortCancel = this.onTimerAbortCancel.bind(this);
        this.onStatisticsClick = this.onStatisticsClick.bind(this);

        this.state = {
            abortTimerModalObject: null,
            timerModalObject: null,
        };
    }

    onIconClick(device: ToggleDeviceModel): void {
        this.props.toggleDevice(device, !device.value);
    }

    onConfigClick(device: DeviceModel): void {
        this.props.history.push("/config/" + device.id);
    }

    onStatisticsClick(device: DeviceModel): void {
        this.props.history.push("/statistics/" + device.id);
    }

    onAutomatedClick(device: ToggleDeviceModel): void {
        this.props.setDeviceAutomated(device, !device.automated);
    }

    onTimerClick(device: ToggleDeviceModel): void {
        this.setState({ ...this.state, timerModalObject: device });
    }

    onTimerAbortClick(device: ToggleDeviceModel): void {
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

    renderDevice(device: DeviceModel): JSX.Element {
        if(device.deviceType === "TOGGLE_DEVICE")
        {
            return (
                <ToggleDevice
                    key={device.id}
                    device={device}
                    onAutomatedClick={this.onAutomatedClick}
                    onIconClick={this.onIconClick}
                    onTimerAbortClick={this.onTimerAbortClick}
                    onTimerClick={this.onTimerClick}
                />
            )
        }
        else if(device.deviceType === "VALUE_DEVICE")
        {
            return (
                <ValueDevice
                    key={device.id}
                    device={device}
                    onStatisticsClick={this.onStatisticsClick}
                />
            )
        }
        else if(device.deviceType === "WEATHER_AIR_DEVICE"){
            return (
                <WeatherAir
                    key={device.id}
                    device={device as WeatherAirDeviceModel}
                    onStatisticsClick={this.onStatisticsClick}
                />
            )
        }
        else if(device.deviceType === "WEATHER_WIND_DEVICE"){
            return (
                <WeatherWindDevice
                    key={device.id}
                    device={device as WeatherWindDeviceModel}
                    onStatisticsClick={this.onStatisticsClick}
                />
            )
        }
        else if(device.deviceType === "SUN_DEVICE"){
            return (
                <SunDevice
                    key={device.id}
                    device={device as SunDeviceViewModel}
                />
            )
        }

        return null;
    }

    public render() {
        return <ContainerDiv>
            {this.props.devices.deviceList.map(function (device, index) {
                return this.renderDevice(device);
            }, this)}

            {!!this.state.timerModalObject && <TimerModal onOkClick={this.onTimerOk} onCancelClick={this.onTimerCancel} />}
            {!!this.state.abortTimerModalObject && <ConfirmationModal onOkClick={this.onTimerAbortOk} onCancelClick={this.onTimerAbortCancel} title="Avbryt timer" message="Är du säker på att du vill avbryta timern?" />}

        </ContainerDiv>;
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
        setTimerDevice: (device: ToggleDeviceModel, time: number, state: boolean) => dispatch(setTimerDevice(device, time, state)),
        abortTimerDevice: (device: ToggleDeviceModel) => dispatch(abortTimerDevice(device)),
        toggleDevice: (device: ToggleDeviceModel, toggled: boolean) => dispatch(toggleDevice(device, toggled)),
        connectToDeviceApi: () => dispatch(connectToDeviceApi()),
        disconnectFromDeviceApi: () => dispatch(disconnectFromDeviceApi()),
        setDeviceAutomated: (device: ToggleDeviceModel, automated: boolean) => dispatch(setDeviceAutomated(device, automated))
    }
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(HomePage));