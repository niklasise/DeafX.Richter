import * as React from "react";
import Modal from "./ModalComponent";
import Button from "./Input/Button";

interface TimerModalState {
    selectedTime: number;
    selectedState: boolean;
}

interface TimerModalProps {
    onOkClick(selectedTime: number, selectedState: boolean): void;
    onCancelClick(): void;
}

export default class TimerModal extends React.Component<TimerModalProps, TimerModalState> {

    private timeoutId: number;
    private isIos: boolean;

    constructor(props: TimerModalProps) {
        super();

        this.state = {
            selectedTime: 0,
            selectedState: true
        };

        this.startClick = this.startClick.bind(this);
        this.stopClick = this.stopClick.bind(this);
        this.onSelectedStateToggle = this.onSelectedStateToggle.bind(this);
        this.isIos = !!navigator.platform && /iPad|iPhone|iPod/.test(navigator.platform);
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

    private onSelectedStateToggle()
    {
        this.setState({
            ...this.state,
            selectedState: !this.state.selectedState
        });
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

        return <Modal title="Välj tid" onCancelClick={this.props.onCancelClick}>
            <div className="timerModalContainer">

                <div className="timerSelectorContainer">
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

                    {this.isIos && <input type="time"/>}
                </div>
                <div className="stateSelectorContainer">
                    <Button additionalClasses="w100px" color={this.state.selectedState ? "green" : "red"} text={this.state.selectedState ? "På" : "Av"} onClicked={this.onSelectedStateToggle} />
                </div>
            </div>

            <div className="fr mr15 mb15">
                <Button text="Ok" additionalClasses="w80px mr15" onClicked={() => { this.props.onOkClick(this.state.selectedTime, this.state.selectedState); }} />
                <Button text="Avbryt" additionalClasses="w80px" onClicked={this.props.onCancelClick}/>
            </div>

        </Modal>
    }

}


