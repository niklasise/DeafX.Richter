import * as React from "react";


interface TimerModalState {
    selectedTime: number;
}

interface TimerModalProps {
    initialTime: number;
    onOkClick(selectedTime: number): void;
    onCancelClick(): void;
    onResetClick(): void;
}

export default class TimerModal extends React.Component<TimerModalProps, TimerModalState> {

    constructor(props: TimerModalProps) {
        super();

        this.state = {
            selectedTime: props.initialTime
        };

        this.incrementSeconds = this.incrementSeconds.bind(this);
        this.decrementSeconds = this.decrementSeconds.bind(this);
        this.incrementMinutes = this.incrementMinutes.bind(this);
        this.decrementMinutes = this.decrementMinutes.bind(this);
        this.incrementHours = this.incrementHours.bind(this);
        this.decrementHours = this.decrementHours.bind(this);
    }

    private incrementSeconds() {
        this.setState({
            ...this.state,
            selectedTime: this.state.selectedTime + 1
        })
    }

    private decrementSeconds() {
        this.setState({
            ...this.state,
            selectedTime: this.state.selectedTime - 1
        })
    }

    private incrementMinutes() {
        this.setState({
            ...this.state,
            selectedTime: this.state.selectedTime + 60
        })
    }

    private decrementMinutes() {
        this.setState({
            ...this.state,
            selectedTime: this.state.selectedTime - 60
        })
    }

    private incrementHours() {
        this.setState({
            ...this.state,
            selectedTime: this.state.selectedTime + 3600
        })
    }

    private decrementHours() {
        this.setState({
            ...this.state,
            selectedTime: this.state.selectedTime - 3600
        })
    }

    public render() {
        let h : any = Math.floor(this.state.selectedTime / 3600);
        let m : any = Math.floor(this.state.selectedTime % 3600 / 60);
        let s : any = Math.floor(this.state.selectedTime % 3600 % 60);

        h = h >= 10 ? h : '0' + h;
        m = m >= 10 ? m : '0' + m;
        s = s >= 10 ? s : '0' + s;

        return <div className="modalOverlay">
            <div className="modal">
                <div className="modalHeader">
                    <span>Välj tid</span>
                    <i className="fa fa-times clickable" onClick={this.props.onCancelClick} />
                </div>

                <div className="modalBody">

                    <div className="timerContainer">

                        <div className="timerSelector">
                            <i className="fa fa-chevron-up clickable" onClick={this.incrementHours}/>
                            <span>{h}</span>
                            <i className="fa fa-chevron-down clickable" onClick={this.decrementHours}/>
                        </div>

                        <span>:</span>

                        <div className="timerSelector">
                            <i className="fa fa-chevron-up clickable" onClick={this.incrementMinutes} />
                            <span>{m}</span>
                            <i className="fa fa-chevron-down clickable" onClick={this.decrementMinutes}/>
                        </div>

                        <span>:</span>

                        <div className="timerSelector">
                            <i className="fa fa-chevron-up clickable" onClick={this.incrementSeconds} />
                            <span>{s}</span>
                            <i className="fa fa-chevron-down clickable" onClick={this.decrementSeconds} />
                        </div>


                    </div>

                    <div className="fl ml15 mb15">
                        <button className="btn red" onClick={this.props.onResetClick}>Nollställ</button>
                    </div>

                    <div className="fr mr15 mb15">
                        <button className="btn mr15" onClick={() => { this.props.onOkClick(this.state.selectedTime); }}>Ok</button>
                        <button className="btn" onClick={this.props.onCancelClick}>Avbryt</button>
                    </div>

                </div>


            </div>
        </div>;
    }

}


