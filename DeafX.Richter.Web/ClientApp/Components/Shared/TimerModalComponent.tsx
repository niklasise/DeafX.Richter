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

    private timeoutId: number;

    constructor(props: TimerModalProps) {
        super();

        this.state = {
            selectedTime: !props.initialTime ? 0 : props.initialTime
        };

        this.startClick = this.startClick.bind(this);
        this.stopClick = this.stopClick.bind(this);
    }

    private startClick(unit: string, decrement: boolean) {

        this.editSelectedTime(unit, decrement);

        this.timeoutId = setTimeout(this.clickTick.bind(this), 500, unit, decrement);
    }

    private stopClick() {
        clearTimeout(this.timeoutId);
        this.timeoutId = 0;
    }

    private clickTick(unit: string, decrement: boolean) {

        this.editSelectedTime(unit, decrement);

        this.timeoutId = setTimeout(this.clickTick.bind(this), 100, unit, decrement);
    }


    private editSelectedTime(unit: string, decrement: boolean) {

        let modifier: number;

        if (unit === 'h')
        {
            modifier = 3600;
        }
        else if (unit === 'm')
        {
            modifier = 60;
        }
        else {
            modifier = 1;
        }

        if (decrement) {
            modifier *= -1;
        }

        let newTime = this.state.selectedTime + modifier;

        this.setState({
            ...this.state,
            selectedTime: newTime > 0 ? newTime : 0
        });
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
                            <i className="fa fa-chevron-up clickable" onMouseDown={() => { this.startClick('h', false); }} onMouseUp={this.stopClick} />
                            <span>{h}</span>
                            <i className="fa fa-chevron-down clickable" onMouseDown={() => { this.startClick('h', true); }} onMouseUp={this.stopClick}/>
                        </div>

                        <span>:</span>

                        <div className="timerSelector">
                            <i className="fa fa-chevron-up clickable" onMouseDown={() => { this.startClick('m', false); }} onMouseUp={this.stopClick}/>
                            <span>{m}</span>
                            <i className="fa fa-chevron-down clickable" onMouseDown={() => { this.startClick('m', true); }} onMouseUp={this.stopClick}/>
                        </div>

                        <span>:</span>

                        <div className="timerSelector">
                            <i className="fa fa-chevron-up clickable" onMouseDown={() => { this.startClick('s', false); }} onMouseUp={this.stopClick}/>
                            <span>{s}</span>
                            <i className="fa fa-chevron-down clickable" onMouseDown={() => { this.startClick('s', true); }} onMouseUp={this.stopClick}/>
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


