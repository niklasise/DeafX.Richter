import * as React from "react";
import Modal from "./ModalComponent";
import Button from "./Input/Button";

export interface ComfirmationModalComponentProps {
    onCancelClick(): void;
    onOkClick(): void;
    title: string;
    message: string;
}

const ComfirmationModalComponent: React.SFC<ComfirmationModalComponentProps> = (props) => {
    return <Modal title={props.title} onCancelClick={props.onCancelClick}>

        <div className="mt20 mb20 tac">
            {props.message}
        </div>

        <div className="fr mr15 mb15">
            <Button text="Ok" additionalClasses="w80px mr15" onClicked={() => { this.props.onOkClick(this.state.selectedTime, this.state.selectedState); }} />
            <Button text="Avbryt" additionalClasses="w80px" onClicked={this.props.onCancelClick} />
        </div>
    </Modal>;
}

export default ComfirmationModalComponent;