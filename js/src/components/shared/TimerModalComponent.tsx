import * as React from "react";
import Modal from "./ModalComponent";
import Button from "./input/Button";
import styled from "styled-components";
import styles from "constants/styles" ;

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
        super(props);

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

        return (
            <Modal title="Välj tid" onCancelClick={this.props.onCancelClick}>
                <ContainerDiv>

                    <SelectorContainerDiv>
                        <SelectorDiv>
                            <ClickableIcon className="fa fa-chevron-up" onMouseDown={() => { this.startClick('h', false); }} onMouseUp={this.stopClick} />
                            <span>{h}</span>
                            <ClickableIcon className="fa fa-chevron-down" onMouseDown={() => { this.startClick('h', true); }} onMouseUp={this.stopClick}/>
                        </SelectorDiv>

                        <span>:</span>

                        <SelectorDiv>
                            <ClickableIcon className="fa fa-chevron-up" onMouseDown={() => { this.startClick('m', false); }} onMouseUp={this.stopClick}/>
                            <span>{m}</span>
                            <ClickableIcon className="fa fa-chevron-down" onMouseDown={() => { this.startClick('m', true); }} onMouseUp={this.stopClick}/>
                        </SelectorDiv>

                        {this.isIos && <input type="time"/>}
                    </SelectorContainerDiv>
                    <span>
                        <StateButton className="w100px" color={this.state.selectedState ? "green" : "red"} text={this.state.selectedState ? "På" : "Av"} onClicked={this.onSelectedStateToggle} />
                    </span>
                </ContainerDiv>

                <ButtonDiv>
                    <OkButton text="Ok" className="w80px mr15" onClicked={() => { this.props.onOkClick(this.state.selectedTime, this.state.selectedState); }} />
                    <AbortButton text="Avbryt" className="w80px" onClicked={this.props.onCancelClick}/>
                </ButtonDiv>

            </Modal>
        );
    }

}

const ButtonDiv = styled.div`
    float:right;
    margin-right: 15px;
    margin-bottom: 15px;
`

const StateButton = styled(Button)`
    width: 100px;
`

const OkButton = styled(Button)`
    width: 80px;
    margin-right: 15px;
`

const AbortButton = styled(Button)`
    width: 80px;
`

const SelectorDiv = styled.div`
    position: relative;
    display: inline;
    margin: 0 auto;

    i:nth-of-type(1) {
        position: absolute;
        top: -20px;
        left: 1px;
    }

    i:nth-of-type(2) {
        position: absolute;
        bottom: -20px;
        left: 1px;
    }
`

const SelectorContainerDiv = styled.div`
    position: relative;
    display: inline;
    margin-right: 20px;
`

const ClickableIcon = styled.i`
    cursor: pointer;
`

const ContainerDiv = styled.div`
    margin: 50px 0;
`


