import * as React from "react";
import Modal from "./ModalComponent";
import Button from "./input/Button";

export interface ConfirmationModalComponentProps {
    onCancelClick(): void;
    onOkClick(): void;
    title: string;
    message: string;
}

const ConfirmationModalComponent: React.SFC<ConfirmationModalComponentProps> = (props) => {
    return <Modal title={props.title} onCancelClick={props.onCancelClick}>

        <div className="confirmationModalContainer">
            {props.message}
        </div>

        <div className="fr mr15 mb15">
            <Button text="Ok" className="w80px mr15" onClicked={() => { props.onOkClick(); }} />
            <Button text="Avbryt" className="w80px" onClicked={props.onCancelClick} />
        </div>
    </Modal>;
}

export default ConfirmationModalComponent;