# 🚀 Stray Swarm - Publishing Guide

Welcome to the **Stray Swarm** Publishing Guide! This document covers everything required to successfully launch our urban animal rescue game on both the Google Play Store and Apple App Store.

---

## 📝 1. Pre-Publishing Checklist

Before submitting **Stray Swarm** to any app store, ensure the game meets the following critical criteria:

### 🎯 Game Completion Requirements
- [ ] **Core Loop:** Swiping, tail formation, and delivery mechanics are fully functional and bug-free.
- [ ] **Content:** All levels, animals, and rescue vans are implemented.
- [ ] **Progression:** 3-star rating system and unlock conditions are working correctly.
- [ ] **Monetization (if applicable):** In-app purchases or ads are tested and integrated properly.
- [ ] **Audio:** Music, SFX, and mixing are balanced and fully implemented.
- [ ] **Input System:** Verify New Input System is properly configured and tested on both platforms.
- [ ] **Profiling:** Test with Unity 6 Profiler on target devices.
- [ ] **API Migration:** Ensure all deprecated API calls have been replaced.

### ⏱️ Performance Benchmarks
| Platform | Target FPS | Max App Size (Download) | Max App Size (Install) | Memory Usage |
| :--- | :--- | :--- | :--- | :--- |
| **Android** | 60 FPS | < 100MB (APK/AAB) | < 300MB | < 512MB |
| **iOS** | 60 FPS | < 200MB (App Store) | < 400MB | < 512MB |

> [!IMPORTANT]
> Stray Swarm uses Unity's URP (Universal Render Pipeline). Ensure all materials are optimized, textures are compressed (ASTC), and object pooling is active to maintain 60 FPS on low-end devices.

### ♿ Accessibility Compliance
- [ ] High contrast mode for UI elements.
- [ ] Colorblind-friendly options (since color matching is a core mechanic).
- [ ] Scalable UI text.

### 🔒 Policy & Rating
- [ ] **Privacy Policy:** Required for both stores. Must be hosted on a live URL.
- [ ] **Age Rating:** Target is PEGI 3 / Everyone. Stray Swarm contains no violence, bad language, or mature themes.

---

## 🤖 2. Google Play Store

### 🔑 Setup & Account
- **Developer Account:** Requires a one-time $25 fee.
- **Console:** Google Play Console is the dashboard for all Android app management.

### 📦 Unity Android Build Settings
```csharp
// Required Unity Build Settings for Google Play
Minimum API Level: Android 7.0 (API 24) // Verify this is the current minimum
Target API Level: Latest (API 34+)
Scripting Backend: IL2CPP (REQUIRED for ARM64, which is MANDATORY for Google Play)
Target Architectures: ARM64 (required), ARMv7 (optional but recommended for older devices)
Build Format: Android App Bundle (.aab) (REQUIRED by Google Play)
Texture Compression: ASTC (recommended for modern devices)
Input System: Unity 6 uses the New Input System by default — ensure it's enabled in Player Settings
```

### ✍️ App Signing
1. **Keystore Creation:** Create a keystore in Unity (`Player Settings > Publishing Settings`). **BACK THIS UP MULTIPLE TIMES.** If lost, updating the app becomes extremely difficult.
2. **Play App Signing:** Opt into "Play App Signing". Google will manage the app's signing key and generate optimized APKs from your AAB.

### 📄 Store Listing Requirements
| Element | Specification | Guidelines for Stray Swarm |
| :--- | :--- | :--- |
| **Title** | Max 30 chars | "Stray Swarm: Animal Rescue" |
| **Short Desc.** | Max 80 chars | "Swipe, collect stray pets, and guide them to safety in this colorful puzzle!" |
| **Full Desc.** | Max 4000 chars | Detail the mechanics (conga line, color matching), cute animals, and puzzle flow. |
| **Screenshots** | Min 2, Max 8 (16:9 or 9:16) | Show UI, conga line gameplay, rescue station, and various animals. |
| **Feature Graphic** | 1024 x 500 px | High-quality art of the Stray Cat leading a colorful swarm. No text if possible. |
| **App Icon** | 512 x 512 px | Bold, rounded Stray Cat face with a bright, contrasting background. |

### 📋 Forms & Questionnaires
- **Content Rating:** Answer the questionnaire honestly (No violence, no gambling, etc.) to get the Everyone/PEGI 3 rating.
- **Data Safety Form:** Detail exactly what data is collected (e.g., analytics, crash logs) and why.
- **Privacy Policy:** Link to the hosted privacy policy.

### 🧪 Testing Tracks & Release Process
1. **Internal Testing:** Upload AAB to the internal track. Invite up to 100 testers via email.
2. **Closed Testing:** Broaden the test to a larger, closed group.
3. **Open Testing:** Beta version available on the Play Store for anyone to opt-in.
4. **Production:** Final release. Use "Staged Rollout" (e.g., 10%, 50%, 100%) to monitor for critical bugs.

---

## 🍎 3. Apple App Store

### 🔑 Setup & Account
- **Developer Program:** Requires a $99/year subscription.
- **Console:** App Store Connect.

### 📦 Unity iOS Build Settings
```csharp
// Required Unity Build Settings for iOS
Minimum iOS Version: 13.0+ (or 15.0+ for latest features)
Architecture: ARM64 only
Scripting Backend: IL2CPP
Bitcode: Disabled (Apple deprecated bitcode in Xcode 14)
Xcode Version: Latest stable
```

> [!NOTE]
> A Mac is required to build and upload the iOS project via Xcode.

### 📜 Certificates, Identifiers & Profiles
1. **App ID:** Create an App ID in the Apple Developer portal (e.g., `com.antigravity.strayswarm`).
2. **Certificates:** Generate a Distribution Certificate via Xcode or Keychain Access.
3. **Provisioning Profile:** Create an App Store Distribution profile linked to the App ID and Certificate.

### 📄 Store Listing Requirements
| Element | Specification | Guidelines for Stray Swarm |
| :--- | :--- | :--- |
| **Name** | Max 30 chars | "Stray Swarm: Animal Rescue" |
| **Subtitle** | Max 30 chars | "Lead cute pets to safety!" |
| **Keywords** | Max 100 chars total | cat,puzzle,rescue,pets,animals,maze,casual,conga,color |
| **Description** | Max 4000 chars | Similar to Play Store, but highly optimized for the first 3 lines (before "Read More"). |
| **Screenshots** | Up to 10 per device | Must provide 6.5" (iPhone Max) and 5.5" (iPhone Plus) at minimum. iPad sizes required if iPad is supported. |
| **App Preview** | 15-30 seconds | High-energy gameplay trailer showing the core loop (swipe, collect, deliver). |
| **App Icon** | 1024 x 1024 px | Same design as Play Store, no alpha channel (transparency). |

### 🧪 TestFlight & Release Process
1. **Upload to App Store Connect:** Build the project in Unity, open in Xcode, archive, and upload.
2. **TestFlight (Internal):** Add up to 100 internal testers (no Apple review required).
3. **TestFlight (External):** Add up to 10,000 external testers (requires a brief Apple Beta review).
4. **App Review:** Submit for production. Ensure the game strictly follows Apple's App Review Guidelines (especially regarding user data, crashes, and performance).
5. **Release:** Manually release or set a scheduled release date.

---

## 🖼️ 4. Store Assets Needed (Summary)

Ensure all assets match the chunky, pastel, and bold aesthetic of **Stray Swarm**.

| Asset Type | Google Play Store Size | Apple App Store Size | Notes |
| :--- | :--- | :--- | :--- |
| **App Icon** | 512x512 | 1024x1024 | Apple icon must have NO transparency. |
| **Feature Graphic** | 1024x500 | N/A | Key branding art. |
| **Phone Screenshots** | 1080x1920 (Recommended) | 1284x2778 (6.5"), 1242x2208 (5.5") | Use localized captions on screenshots. |
| **Tablet Screenshots**| 1536x2048 (Recommended) | 2048x2732 (12.9" iPad) | Required if supporting tablets. |
| **Promo Video** | YouTube Link (16:9) | 1920x1080 (15-30s) | Apple requires mostly raw gameplay. |

> [!TIP]
> **Screenshot Strategy:** Don't just upload raw gameplay. Add visually appealing backgrounds, devices frames (optional but popular), and short, punchy captions (e.g., "Build the longest conga line!", "Match colors to rescue!").

---

## 📈 5. ASO (App Store Optimization)

App Store Optimization is crucial for organic discovery.

### 🔍 Keyword Strategy
- **Primary Keywords:** puzzle, casual, cat, rescue, animals, conga line.
- **Long-tail Keywords:** "cat puzzle game", "animal rescue maze", "cute pet matching".
- **Implementation:**
  - **iOS:** Stuff the 100-character keyword field. Don't repeat words. Use commas, no spaces.
  - **Android:** Incorporate keywords naturally throughout the title, short description, and full description (aim for 2-3% keyword density in the full description).

### ✍️ Title & Description Optimization
- The Title carries the most ASO weight on both platforms. Include the strongest keyword (e.g., "Stray Swarm: **Animal Rescue Puzzle**" if character limits allow, though usually sticking to a branded subtitle is safer).
- The first three lines of the description must contain the core hook, as most users don't click "Read More".

### 🌟 Ratings and Reviews
- Implement a native in-app rating prompt (using Unity's native review plugins).
- **Trigger point:** Ask for a rating *only* after a positive moment, such as completing a difficult level with 3 stars or unlocking a new animal type. **Never interrupt gameplay.**

---

## 🚀 6. Post-Launch

The launch is just the beginning. Continuous monitoring and updates are required.

### 📊 Analytics & Crash Reporting
- **Firebase Analytics / Unity Analytics:** Track core KPIs (Day 1, Day 7 Retention, Session Length, Level Completion Rates).
- **Firebase Crashlytics:** Monitor ANRs (Application Not Responding) and crash rates. Both Google and Apple penalize apps with crash rates > 1.09%.

### 🔄 Update Cadence
- Aim for an update every 2-4 weeks post-launch.
- **Update Content:** New levels, seasonal themes (e.g., Halloween pets), performance improvements, bug fixes.

### 👂 User Feedback
- Monitor store reviews daily.
- Respond to negative reviews politely and constructively.
- Use feedback to drive the product roadmap (e.g., if players find the Purple Bunny levels too hard, tweak the level design).

### 🔀 A/B Testing
- Use **Google Play Store Listing Experiments** to A/B test the app icon, feature graphic, and screenshots to optimize conversion rates.
- Test one element at a time (e.g., Icon A vs. Icon B) for accurate data.

---

## ⚖️ 7. Legal Requirements

Compliance is mandatory to prevent store rejection or legal action.

### 🛡️ Privacy Policy
- A comprehensive privacy policy is required.
- Must explain what data is collected (Unity Analytics, Firebase, Ads), how it is used, and how users can request data deletion.
- Host the policy on the game's website or a dedicated service (e.g., Termly, Iubenda).

### 📜 Terms of Service
- Defines the rules for using the app, IP ownership, and limits liability.

### 🔐 Privacy & Data Safety (2026 Guidelines)
- **Google Play Data Safety Form:** This form is mandatory. You must accurately declare all data your app collects and shares.
- **Apple App Privacy:** Mandatory declarations in App Store Connect regarding data collection.
- **Analytics & Tracking:** If using Unity Analytics or Firebase, you must explicitly declare data collection in both stores.
- **No Ads/Analytics:** If launching with no ads or analytics initially, declare 'No data collected' to significantly simplify the review process.

### 👶 COPPA & GDPR Considerations
- **COPPA (Children's Online Privacy Protection Act):** Since Stray Swarm appeals to children (cute animals, colorful art), you must comply with COPPA. Ensure any ad networks used are configured to serve family-friendly, non-personalized ads. Avoid collecting PII (Personally Identifiable Information).
- **GDPR (General Data Protection Regulation):** For EU players, implement a Consent Management Platform (CMP) to gather explicit consent before tracking analytics or serving personalized ads. Unity and Google AdMob provide tools for this.

---
*End of Publishing Guide. Review periodically as store policies change frequently.*
