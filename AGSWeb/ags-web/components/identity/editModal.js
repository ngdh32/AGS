import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';


export default function EditModal({ toggle, isOpen, children }) {
    return (
        <Modal isOpen={isOpen} toggle={toggle} keyboard={false} backdrop={false}>
            {
                children
            }
        </Modal>
    )
}