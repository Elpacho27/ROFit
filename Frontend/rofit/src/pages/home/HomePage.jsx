import "../../styles/home/HomePage.css";
import gyms4you from "../../assets/images/gyms4you2.jpg";
import blueGym from "../../assets/images/blue_gym.jpg";
import stayFit from "../../assets/images/stay_fit.jpg";

//MOCK ONLY, to be replaced with API data in the future
function HomePage() {
  const gyms = [
    {
      id: 1,
      name: "Gyms4You",
      address: "Trg Slobode 6, Osijek",
      image: gyms4you,
    },
    {
      id: 2,
      name: "Blue Gym Centar",
      address: "Ul. Pavla Pejačevića 22, Osijek",
      image: blueGym,
    },
    {
      id: 3,
      name: "Stay Fit",
      address: "Vukovarska Cesta 31, Osijek",
      image: stayFit,
    },
  ];

  const memberships = [
    {
      id: 1,
      name: "Starter",
      price: "29 € / month",
      description:
        "2 workouts per week, perfect to get started and build a habit.",
      features: ["Gym access", "Basic training plan", "Online support"],
    },
    {
      id: 2,
      name: "Progress",
      price: "49 € / month",
      description: "4–5 workouts per week for steady progress.",
      features: [
        "Gym access",
        "Personalized plan",
        "Chat with your coach",
        "Progress tracking",
      ],
    },
    {
      id: 3,
      name: "Elite",
      price: "79 € / month",
      description: "Full support for maximum results.",
      features: [
        "Everything in Progress",
        "Weekly 1:1 online check‑in",
        "Advanced progress analytics",
      ],
    },
  ];

  const steps = [
    {
      id: 1,
      title: "Choose your gym",
      text: "Pick the location that fits your routine and schedule.",
    },
    {
      id: 2,
      title: "Pick a membership",
      text: "Select a plan that matches your goals and experience.",
    },
    {
      id: 3,
      title: "Work with your coach",
      text: "Follow your plan, send updates, and stay in touch via chat.",
    },
  ];

  return (
    <div className="home-root">
      <section className="home-hero">
        <div className="home-hero-content">
          <h1 className="home-hero-title">
            Stay consistent, we handle the rest.
          </h1>
          <p className="home-hero-subtitle">
            We connect you, your coach, and your gym in one simple system – no
            spreadsheets, no chaos, just clear progress.
          </p>
        </div>
      </section>

      <section className="home-section">
        <div className="home-section-header">
          <h2 className="home-section-title">Partner gyms</h2>
          <p className="home-section-subtitle">
            Choose a location, we take care of the structure and coaching.
          </p>
        </div>

        <div className="home-gyms-grid">
          {gyms.map((gym) => (
            <div key={gym.id} className="home-gym-card">
              <div className="home-gym-image-wrapper">
                <img
                  src={gym.image}
                  alt={gym.name}
                  className="home-gym-image"
                />
              </div>
              <div className="home-gym-body">
                <h3 className="home-gym-name">{gym.name}</h3>
                <p className="home-gym-address">{gym.address}</p>
              </div>
            </div>
          ))}
        </div>
      </section>

      <section className="home-section">
        <div className="home-section-header">
          <h2 className="home-section-title">Membership plans</h2>
          <p className="home-section-subtitle">
            Flexible options for different goals – from first workout to
            competition prep.
          </p>
        </div>

        <div className="home-memberships-grid">
          {memberships.map((m) => (
            <div
              key={m.id}
              className={`home-membership-card ${
                m.name === "Progress" ? "home-membership-card--featured" : ""
              }`}
            >
              <div className="home-membership-header">
                <h3 className="home-membership-name">{m.name}</h3>
                <p className="home-membership-price">{m.price}</p>
              </div>
              <p className="home-membership-description">{m.description}</p>
              <ul className="home-membership-features">
                {m.features.map((f) => (
                  <li key={f}>{f}</li>
                ))}
              </ul>
            </div>
          ))}
        </div>
      </section>

      <section className="home-section">
        <div className="home-section-header">
          <h2 className="home-section-title">How ROFit works</h2>
          <p className="home-section-subtitle">
            Simple for you, powerful for your coach and gym team.
          </p>
        </div>

        <div className="home-steps-grid">
          {steps.map((step) => (
            <div key={step.id} className="home-step-card">
              <div className="home-step-number">0{step.id}</div>
              <h3 className="home-step-title">{step.title}</h3>
              <p className="home-step-text">{step.text}</p>
            </div>
          ))}
        </div>
      </section>
    </div>
  );
}

export default HomePage;
