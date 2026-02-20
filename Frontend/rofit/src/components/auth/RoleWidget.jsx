import UserStore from "../../stores/UserStore";

function RoleWidget({ allowedRoles, children }) {
  const user = UserStore.getCurrentUser();

  if (!user) return null;

  if (!allowedRoles.includes(user.role)) {
    return null;
  }

  return <>{children}</>;
}

export default RoleWidget;
