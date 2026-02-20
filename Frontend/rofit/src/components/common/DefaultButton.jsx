import "../../styles/common/DefaultButton.css";
function DefaultButton({ title, onClick, disabled, type = "button" }) {
  return (
    <button
      className="default-button"
      onClick={onClick}
      disabled={disabled}
      type={type}
    >
      {title}
    </button>
  );
}
export default DefaultButton;
