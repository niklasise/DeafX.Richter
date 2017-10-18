import * as React from "react";
import { Device as DeviceModel, ToggleDevice } from "../../Models/Device";

export interface DeviceTimerProps {
    device: DeviceModel;
    timerValue: number;
    onTimerClick(source: DeviceModel, timeLeft: number): void;
}

interface DeviceTimerState {
    timeLeft: number;
}
//const DeviceTimer: React.SFC<DeviceClockProps> = (props) => {
//    function formatTimerValue(seconds: number): string
//    {
//        let h: any = Math.floor(seconds / 3600);
//        let m: any = Math.floor(seconds % 3600 / 60);
//        let s: any = Math.floor(seconds % 3600 % 60);

//        h = h >= 10 ? h : '0' + h;
//        m = m >= 10 ? m : '0' + m;
//        s = s >= 10 ? s : '0' + s;

//        return h + ':' + m + ':' + s;
//    }

//    return <div className="clickable" onClick={(e) => { e.stopPropagation(); props.onTimerClick(props.device); }}>
//        {!!props.timerValue &&
//            <div>
//                <i className="fa fa-clock-o" onClick={(e) => { e.stopPropagation(); props.onTimerClick(props.device); }} />
//                <span>{formatTimerValue(props.timerValue)}</span>
//            </div>
//        }
//        {!props.timerValue && <i className="fa fa-clock-o" onClick={(e) => { e.stopPropagation(); props.onTimerClick(props.device); }} />}

//    </div>;
//}

//export default DeviceTimer;

export default class DeviceTimer extends React.Component<DeviceTimerProps, DeviceTimerState> {

    private timeoutId: number;
    private timeMounted: number;

    constructor(props: DeviceTimerProps) {
        super();

        this.tick = this.tick.bind(this);

        this.state = {
            timeLeft: props.timerValue
        };
    }

    componentDidMount() {
        this.timeMounted = this.getTime();
        this.timeoutId = setTimeout(this.tick, 1000);
    }

    componentWillUnmount() {
        clearTimeout(this.timeoutId);
    }

    componentWillReceiveProps(nextProps: DeviceTimerProps) {
        if (nextProps.timerValue === this.props.timerValue)
        {
            return;
        }

        this.timeMounted = this.getTime();

        this.setState({
            ...this.state,
            timeLeft: this.getTimeLeft(nextProps.timerValue)
        });

        if (this.timeoutId === 0) {
            this.timeoutId = setTimeout(this.tick, 1000);
        }
    }

    tick() {
        this.setState({
            ...this.state,
            timeLeft: this.getTimeLeft(this.props.timerValue)
        });

        if (this.state.timeLeft > 0)
        {
            this.timeoutId = setTimeout(this.tick, 1000);
        }
        else
        {
            this.timeoutId = 0;    
        }
    }

    getTimeLeft(timerValue: number): number {
        let timeLeft = this.timeMounted + timerValue - this.getTime();

        return timeLeft > 0 ? timeLeft : 0;
    }

    getTime() : number {
        return Math.floor(Date.now() / 1000);
    }

    formatTimerValue(seconds: number): string {
        let h: any = Math.floor(seconds / 3600);
        let m: any = Math.floor(seconds % 3600 / 60);
        let s: any = Math.floor(seconds % 3600 % 60);

        h = h >= 10 ? h : '0' + h;
        m = m >= 10 ? m : '0' + m;
        s = s >= 10 ? s : '0' + s;

        return h + ':' + m + ':' + s;
    }

    public render() {
        return <div className="clickable" onClick={(e) => { e.stopPropagation(); this.props.onTimerClick(this.props.device, this.state.timeLeft); }}>
                <i className="fa fa-clock-o" onClick={(e) => { e.stopPropagation(); this.props.onTimerClick(this.props.device, this.state.timeLeft); }} />
                {!!this.state.timeLeft && <span>{this.formatTimerValue(this.state.timeLeft)}</span>}
        </div>;
    }

}


