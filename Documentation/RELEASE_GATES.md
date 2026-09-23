# Release Gates

No build may be described as “ready for Google Play” or “ready for the App Store” until all required gates are checked with evidence.

## Gameplay

- [ ] All 30 campaign levels are playable and balanced.
- [ ] All advertised heroes, units, evolutions, elements, bosses, and synergies work.
- [ ] Tutorial, victory, defeat, progression, collection, quests, and offline rewards work.
- [ ] A complete new-player-to-final-boss playthrough has been performed.

## Quality

- [ ] EditMode and PlayMode tests pass.
- [ ] No game-breaking or release-blocking defects remain.
- [ ] Save corruption, migration, app termination, and duplicate-reward cases pass.
- [ ] Real Android device matrix passes on low-, mid-, and high-tier devices.
- [ ] Real iPhone/iPad matrix passes on supported OS versions.
- [ ] Stable target frame rate and memory limits are documented.

## Platform and compliance

- [ ] Android AAB is signed with the owner-controlled upload key.
- [ ] iOS archive is signed through the owner-controlled Apple team.
- [ ] Privacy policy, terms, support page, age rating, and data disclosures are approved.
- [ ] Consent flow is verified in applicable regions.
- [ ] Ads and analytics use production IDs only in release builds.
- [ ] Store screenshots, description, icon, feature graphic, and review notes are complete.

## Release control

- [ ] Internal/closed testing is complete.
- [ ] Crash and analytics dashboards show no release blocker.
- [ ] The owner has played and approved the exact signed release candidate.
- [ ] A rollback/hotfix procedure is documented.

