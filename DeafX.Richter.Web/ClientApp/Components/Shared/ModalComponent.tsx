import * as React from "react";

export interface ModalComponentProps {
    onCancelClick(): void;
    title: string
}

const ModalComponent: React.SFC<ModalComponentProps> = (props) => {
    return <div className="modalOverlay">
        <div className="modal">
            <div className="modalHeader">
                <span>{props.title}</span>
                <i className="fa fa-times clickable" onClick={props.onCancelClick} />
            </div>
            <div className="modalBody">

                {props.children}

            </div>
        </div>
    </div>;
}

export default ModalComponent;



