import * as React from "react";
import Modal from "./ModalComponent";
import Button from "./input/Button";
import styled from "styled-components";

export interface ConfirmationModalComponentProps {
    onCancelClick(): void;
    onOkClick(): void;
    title: string;
    message: string;
}

const ConfirmationModalComponent: React.SFC<ConfirmationModalComponentProps> = (props) => {
    return <Modal title={props.title} onCancelClick={props.onCancelClick}>

        <ContainerDiv className="confirmationModalContainer">
            {props.message}
        </ContainerDiv>

        <ButtonContainerDiv>
            <OkButton text="Ok" className="w80px mr15" onClicked={() => { props.onOkClick(); }} />
            <CancelButton text="Avbryt" className="w80px" onClicked={props.onCancelClick} />
        </ButtonContainerDiv>
    </Modal>;
}

const OkButton = styled(Button)`
    margin-right: 15px;
    width: 80px;
`

const CancelButton = styled(Button)`
    width: 80px;
`

const ContainerDiv = styled.div`
    margin: 30px 0;
    font-size: 13px;
    text-align: center;
`

const ButtonContainerDiv = styled.div`
    float: right;
    margin-right: 15px;
    margin-bottom: 15px;
`

export default ConfirmationModalComponent;